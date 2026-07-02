using System.Data;
using Microsoft.Data.SqlClient;
using SalesPro.Contracts.Ordenes;
using SalesPro.Data.Infrastructure;
using SalesPro.Domain.Exceptions;

namespace SalesPro.Data.Repositories;

public sealed class OrdenRepository : IOrdenRepository
{
    // La capa de datos no conoce strings de conexión directamente.
    // Recibe una fábrica para abrir conexiones y así queda más fácil probar/cambiar la configuración.
    private readonly ISqlConnectionFactory _connectionFactory;

    public OrdenRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<OrdenDto> CrearOrdenAsync(CrearOrdenRequest request, decimal porcentajeImpuestoVenta, CancellationToken cancellationToken)
    {
        // Todo lo que modifica la orden se hace con la misma conexión y la misma transacción.
        // Esa es la parte importante para defender: encabezado, detalles e inventario viajan juntos.
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
        var committed = false;

        try
        {
            // Antes de insertar, se valida que el cliente exista y esté activo.
            // Si falla, todavía no se ha tocado inventario ni detalle.
            await ValidarClienteAsync(connection, transaction, request.ClienteId, cancellationToken);

            if (request.EmpleadoId is not null)
            {
                // El empleado es opcional en el contrato, pero si viene informado debe existir.
                await ValidarEmpleadoAsync(connection, transaction, request.EmpleadoId.Value, cancellationToken);
            }

            // Primero se calcula toda la orden en memoria.
            // Así evitamos insertar una orden a medias si algún producto no existe o no tiene stock.
            var detallesCalculados = new List<DetalleCalculado>();
            foreach (var detalle in request.Detalles)
            {
                // UPDLOCK/ROWLOCK bloquea la fila del producto mientras dura la transacción.
                // Esto evita que dos órdenes descuenten el mismo stock al mismo tiempo.
                var producto = await ObtenerProductoBloqueadoAsync(connection, transaction, detalle.ProductoId, cancellationToken);
                if (producto is null)
                {
                    throw new NotFoundException($"El producto {detalle.ProductoId} no existe.");
                }

                if (!producto.PuedeVenderse)
                {
                    throw new ConflictException($"El producto '{producto.Nombre}' no está habilitado para venta.");
                }

                if (producto.ExistenciaEnStock < detalle.Cantidad)
                {
                    throw new ConflictException($"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.ExistenciaEnStock}, solicitado: {detalle.Cantidad}.");
                }

                // El impuesto se calcula solo para productos que lo tienen marcado en base de datos.
                var subtotal = producto.PrecioUnitario * detalle.Cantidad;
                var impuesto = producto.TieneImpuesto
                    ? Math.Round(subtotal * porcentajeImpuestoVenta / 100m, 2, MidpointRounding.AwayFromZero)
                    : 0m;

                detallesCalculados.Add(new DetalleCalculado(
                    producto.ProductoId,
                    producto.Nombre,
                    producto.PrecioUnitario,
                    detalle.Cantidad,
                    subtotal,
                    impuesto));
            }

            var subtotalOrden = detallesCalculados.Sum(d => d.Subtotal);
            var impuestoOrden = detallesCalculados.Sum(d => d.Impuesto);
            var totalOrden = subtotalOrden + impuestoOrden;

            // El encabezado se inserta primero porque SQL Server genera el número de orden.
            // Ese número se usa luego como FK en cada línea de detalle.
            var numeroOrden = await InsertarEncabezadoAsync(
                connection,
                transaction,
                request.ClienteId,
                request.EmpleadoId,
                totalOrden,
                impuestoOrden,
                cancellationToken);

            foreach (var detalle in detallesCalculados)
            {
                // Orden correcto: descontar stock y registrar detalle dentro de la misma transacción.
                // Si cualquiera de los dos falla, el catch hace rollback de todo.
                await DescontarInventarioAsync(connection, transaction, detalle.ProductoId, detalle.Cantidad, cancellationToken);
                await InsertarDetalleAsync(connection, transaction, numeroOrden, detalle, cancellationToken);
            }

            // A partir de aquí la venta queda confirmada en base de datos.
            transaction.Commit();
            committed = true;

            return await ObtenerPorNumeroAsync(numeroOrden, cancellationToken)
                   ?? throw new NotFoundException($"No se pudo recuperar la orden {numeroOrden} después de crearla.");
        }
        catch
        {
            if (!committed)
            {
                // Este rollback es lo que garantiza que no quede inventario descontado sin orden,
                // ni orden creada sin sus detalles.
                transaction.Rollback();
            }

            throw;
        }
    }

    public async Task<OrdenDto?> ObtenerPorNumeroAsync(int numeroOrden, CancellationToken cancellationToken)
    {
        // Se consulta el encabezado y luego los detalles. Se hace separado para devolver un DTO limpio
        // con la estructura maestro-detalle que consume el WPF.
        const string headerSql = """
            SELECT o.numero_orden,
                   o.fk_cliente,
                   CONCAT(c.nombre, ' ', c.apellidos) AS cliente_nombre,
                   o.fecha_orden,
                   o.fk_empleado,
                   o.impuesto,
                   o.total_orden
            FROM Pos_Orden o
            INNER JOIN Cliente c ON c.id = o.fk_cliente
            WHERE o.numero_orden = @numeroOrden;
            """;

        const string detailSql = """
            SELECT d.fk_producto,
                   p.nombre_etiqueta,
                   d.precio_unitario,
                   d.cantidad,
                   d.precio_subtotal,
                   d.impuesto
            FROM Pos_Orden_Detalle d
            INNER JOIN Producto p ON p.product_id = d.fk_producto
            WHERE d.fk_pos_orden = @numeroOrden
            ORDER BY p.nombre_etiqueta;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var headerCommand = new SqlCommand(headerSql, connection);
        headerCommand.Parameters.AddWithValue("@numeroOrden", numeroOrden);

        int clienteId;
        string clienteNombre;
        DateTime fechaOrden;
        int? empleadoId;
        decimal impuesto;
        decimal total;

        await using (var reader = await headerCommand.ExecuteReaderAsync(cancellationToken))
        {
            if (!await reader.ReadAsync(cancellationToken))
            {
                return null;
            }

            clienteId = reader.GetInt32(1);
            clienteNombre = reader.GetString(2);
            fechaOrden = reader.GetDateTime(3);
            empleadoId = reader.IsDBNull(4) ? null : reader.GetInt32(4);
            impuesto = reader.GetDecimal(5);
            total = reader.GetDecimal(6);
        }

        await using var detailCommand = new SqlCommand(detailSql, connection);
        detailCommand.Parameters.AddWithValue("@numeroOrden", numeroOrden);
        await using var detailReader = await detailCommand.ExecuteReaderAsync(cancellationToken);

        var detalles = new List<OrdenDetalleDto>();
        while (await detailReader.ReadAsync(cancellationToken))
        {
            detalles.Add(new OrdenDetalleDto(
                detailReader.GetInt32(0),
                detailReader.GetString(1),
                detailReader.GetDecimal(2),
                detailReader.GetInt32(3),
                detailReader.GetDecimal(4),
                detailReader.GetDecimal(5)));
        }

        return new OrdenDto(
            numeroOrden,
            clienteId,
            clienteNombre,
            fechaOrden,
            empleadoId,
            total - impuesto,
            impuesto,
            total,
            detalles);
    }

    private static async Task ValidarClienteAsync(SqlConnection connection, SqlTransaction transaction, int clienteId, CancellationToken cancellationToken)
    {
        // Validación mínima de integridad antes de vender: cliente existente y activo.
        const string sql = "SELECT COUNT(1) FROM Cliente WHERE id = @clienteId AND activo = 1;";
        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@clienteId", clienteId);
        var count = Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));

        if (count == 0)
        {
            throw new NotFoundException($"El cliente {clienteId} no existe o no está activo.");
        }
    }

    private static async Task ValidarEmpleadoAsync(SqlConnection connection, SqlTransaction transaction, int empleadoId, CancellationToken cancellationToken)
    {
        // Si el empleado viene en la solicitud, también debe estar activo.
        const string sql = "SELECT COUNT(1) FROM Empleado WHERE id = @empleadoId AND activo = 1;";
        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@empleadoId", empleadoId);
        var count = Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));

        if (count == 0)
        {
            throw new NotFoundException($"El empleado {empleadoId} no existe o no está activo.");
        }
    }

    private static async Task<ProductoBloqueado?> ObtenerProductoBloqueadoAsync(SqlConnection connection, SqlTransaction transaction, int productoId, CancellationToken cancellationToken)
    {
        // El bloqueo se pide desde el SELECT, antes de calcular o descontar.
        // Esto es clave en escenarios concurrentes: otro usuario debe esperar a que esta transacción termine.
        const string sql = """
            SELECT product_id,
                   nombre_etiqueta,
                   precio_neto,
                   existencia_en_stock,
                   puede_venderse,
                   tiene_impuesto
            FROM Producto WITH (UPDLOCK, ROWLOCK)
            WHERE product_id = @productoId;
            """;

        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@productoId", productoId);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return new ProductoBloqueado(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetDecimal(2),
            reader.GetInt32(3),
            reader.GetBoolean(4),
            reader.GetBoolean(5));
    }

    private static async Task<int> InsertarEncabezadoAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        int clienteId,
        int? empleadoId,
        decimal totalOrden,
        decimal impuesto,
        CancellationToken cancellationToken)
    {
        // OUTPUT INSERTED permite obtener el número de orden generado sin hacer otra consulta.
        const string sql = """
            INSERT INTO Pos_Orden (fk_cliente, fecha_orden, fk_empleado, total_orden, impuesto)
            OUTPUT INSERTED.numero_orden
            VALUES (@clienteId, SYSDATETIME(), @empleadoId, @totalOrden, @impuesto);
            """;

        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@clienteId", clienteId);
        command.Parameters.AddWithValue("@empleadoId", empleadoId is null ? DBNull.Value : empleadoId.Value);
        command.Parameters.AddWithValue("@totalOrden", totalOrden);
        command.Parameters.AddWithValue("@impuesto", impuesto);

        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }

    private static async Task DescontarInventarioAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        int productoId,
        int cantidad,
        CancellationToken cancellationToken)
    {
        // La condición existencia_en_stock >= @cantidad es una defensa extra:
        // aunque el stock haya sido validado antes, el UPDATE no descuenta si ya no alcanza.
        const string sql = """
            UPDATE Producto
            SET existencia_en_stock = existencia_en_stock - @cantidad
            WHERE product_id = @productoId
              AND puede_venderse = 1
              AND existencia_en_stock >= @cantidad;
            """;

        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@productoId", productoId);
        command.Parameters.AddWithValue("@cantidad", cantidad);

        var rows = await command.ExecuteNonQueryAsync(cancellationToken);
        if (rows != 1)
        {
            // Si no se afectó exactamente una fila, algo no cuadra y se fuerza rollback.
            throw new ConflictException($"No se pudo descontar inventario del producto {productoId}. La transacción será revertida.");
        }
    }

    private static async Task InsertarDetalleAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        int numeroOrden,
        DetalleCalculado detalle,
        CancellationToken cancellationToken)
    {
        // Cada línea guarda los montos calculados al momento de la venta.
        // Así la orden conserva su precio histórico aunque luego cambie el precio del producto.
        const string sql = """
            INSERT INTO Pos_Orden_Detalle
                (fk_pos_orden, fk_producto, impuesto, cantidad, descuento, precio_unitario, precio_subtotal)
            VALUES
                (@numeroOrden, @productoId, @impuesto, @cantidad, 0, @precioUnitario, @subtotal);
            """;

        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@numeroOrden", numeroOrden);
        command.Parameters.AddWithValue("@productoId", detalle.ProductoId);
        command.Parameters.AddWithValue("@impuesto", detalle.Impuesto);
        command.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
        command.Parameters.AddWithValue("@precioUnitario", detalle.PrecioUnitario);
        command.Parameters.AddWithValue("@subtotal", detalle.Subtotal);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    // Records internos para transportar datos dentro del repositorio sin exponer clases extra al resto del sistema.
    private sealed record ProductoBloqueado(
        int ProductoId,
        string Nombre,
        decimal PrecioUnitario,
        int ExistenciaEnStock,
        bool PuedeVenderse,
        bool TieneImpuesto);

    private sealed record DetalleCalculado(
        int ProductoId,
        string NombreProducto,
        decimal PrecioUnitario,
        int Cantidad,
        decimal Subtotal,
        decimal Impuesto);
}
