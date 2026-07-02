using System.Data;
using Microsoft.Data.SqlClient;
using SalesPro.Contracts.Ordenes;
using SalesPro.Data.Infrastructure;
using SalesPro.Domain.Exceptions;

namespace SalesPro.Data.Repositories;

public sealed class OrdenRepository : IOrdenRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public OrdenRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<OrdenDto> CrearOrdenAsync(CrearOrdenRequest request, decimal porcentajeImpuestoVenta, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
        var committed = false;

        try
        {
            await ValidarClienteAsync(connection, transaction, request.ClienteId, cancellationToken);

            if (request.EmpleadoId is not null)
            {
                await ValidarEmpleadoAsync(connection, transaction, request.EmpleadoId.Value, cancellationToken);
            }

            var detallesCalculados = new List<DetalleCalculado>();
            foreach (var detalle in request.Detalles)
            {
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
                await DescontarInventarioAsync(connection, transaction, detalle.ProductoId, detalle.Cantidad, cancellationToken);
                await InsertarDetalleAsync(connection, transaction, numeroOrden, detalle, cancellationToken);
            }

            transaction.Commit();
            committed = true;

            return await ObtenerPorNumeroAsync(numeroOrden, cancellationToken)
                   ?? throw new NotFoundException($"No se pudo recuperar la orden {numeroOrden} después de crearla.");
        }
        catch
        {
            if (!committed)
            {
                transaction.Rollback();
            }

            throw;
        }
    }

    public async Task<OrdenDto?> ObtenerPorNumeroAsync(int numeroOrden, CancellationToken cancellationToken)
    {
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

    public async Task<IReadOnlyCollection<OrdenDto>> ListarAsync(CancellationToken cancellationToken)
    {
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
            ORDER BY o.numero_orden;
            """;

        const string detailSql = """
            SELECT d.fk_pos_orden,
                   d.fk_producto,
                   p.nombre_etiqueta,
                   d.precio_unitario,
                   d.cantidad,
                   d.precio_subtotal,
                   d.impuesto
            FROM Pos_Orden_Detalle d
            INNER JOIN Producto p ON p.product_id = d.fk_producto
            ORDER BY d.fk_pos_orden, p.nombre_etiqueta;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var detallesPorOrden = new Dictionary<int, List<OrdenDetalleDto>>();
        await using (var detailCommand = new SqlCommand(detailSql, connection))
        await using (var detailReader = await detailCommand.ExecuteReaderAsync(cancellationToken))
        {
            while (await detailReader.ReadAsync(cancellationToken))
            {
                var numeroOrden = detailReader.GetInt32(0);
                if (!detallesPorOrden.TryGetValue(numeroOrden, out var lista))
                {
                    lista = new List<OrdenDetalleDto>();
                    detallesPorOrden[numeroOrden] = lista;
                }

                lista.Add(new OrdenDetalleDto(
                    detailReader.GetInt32(1),
                    detailReader.GetString(2),
                    detailReader.GetDecimal(3),
                    detailReader.GetInt32(4),
                    detailReader.GetDecimal(5),
                    detailReader.GetDecimal(6)));
            }
        }

        var ordenes = new List<OrdenDto>();
        await using (var headerCommand = new SqlCommand(headerSql, connection))
        await using (var headerReader = await headerCommand.ExecuteReaderAsync(cancellationToken))
        {
            while (await headerReader.ReadAsync(cancellationToken))
            {
                var numeroOrden = headerReader.GetInt32(0);
                var impuesto = headerReader.GetDecimal(5);
                var total = headerReader.GetDecimal(6);

                var detalles = detallesPorOrden.TryGetValue(numeroOrden, out var lista)
                    ? (IReadOnlyCollection<OrdenDetalleDto>)lista
                    : Array.Empty<OrdenDetalleDto>();

                ordenes.Add(new OrdenDto(
                    numeroOrden,
                    headerReader.GetInt32(1),
                    headerReader.GetString(2),
                    headerReader.GetDateTime(3),
                    headerReader.IsDBNull(4) ? null : headerReader.GetInt32(4),
                    total - impuesto,
                    impuesto,
                    total,
                    detalles));
            }
        }

        return ordenes;
    }

    private static async Task ValidarClienteAsync(SqlConnection connection, SqlTransaction transaction, int clienteId, CancellationToken cancellationToken)
    {
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
