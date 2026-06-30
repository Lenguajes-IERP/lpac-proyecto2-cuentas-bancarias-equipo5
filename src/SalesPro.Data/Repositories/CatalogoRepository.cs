using Microsoft.Data.SqlClient;
using SalesPro.Contracts.Catalogos;
using SalesPro.Data.Infrastructure;

namespace SalesPro.Data.Repositories;

public sealed class CatalogoRepository : ICatalogoRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CatalogoRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<BancoDto>> ListarBancosAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT id, codigo_identificador_banco, nombre
            FROM Banco
            WHERE active = 1
            ORDER BY nombre;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var bancos = new List<BancoDto>();
        while (await reader.ReadAsync(cancellationToken))
        {
            bancos.Add(new BancoDto(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2)));
        }

        return bancos;
    }

    public async Task<IReadOnlyCollection<CompaniaDto>> ListarCompaniasAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT id, nombre, cedula_juridica
            FROM Compania
            ORDER BY nombre;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var companias = new List<CompaniaDto>();
        while (await reader.ReadAsync(cancellationToken))
        {
            companias.Add(new CompaniaDto(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.IsDBNull(2) ? null : reader.GetString(2)));
        }

        return companias;
    }

    public async Task<IReadOnlyCollection<ClienteDto>> BuscarClientesAsync(string? buscar, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT TOP (25) id, nombre, apellidos, numero_identificacion, email
            FROM Cliente
            WHERE activo = 1
              AND (
                    @buscar IS NULL
                    OR nombre LIKE '%' + @buscar + '%'
                    OR apellidos LIKE '%' + @buscar + '%'
                    OR numero_identificacion LIKE '%' + @buscar + '%'
                  )
            ORDER BY nombre, apellidos;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@buscar", string.IsNullOrWhiteSpace(buscar) ? DBNull.Value : buscar.Trim());

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var clientes = new List<ClienteDto>();
        while (await reader.ReadAsync(cancellationToken))
        {
            clientes.Add(new ClienteDto(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4)));
        }

        return clientes;
    }

    public async Task<IReadOnlyCollection<ProductoDto>> BuscarProductosAsync(string? buscar, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT TOP (25)
                   product_id,
                   codigo_barra,
                   nombre_etiqueta,
                   description,
                   existencia_en_stock,
                   precio_neto,
                   puede_venderse,
                   tiene_impuesto
            FROM Producto
            WHERE puede_venderse = 1
              AND (
                    @buscar IS NULL
                    OR nombre_etiqueta LIKE '%' + @buscar + '%'
                    OR codigo_barra LIKE '%' + @buscar + '%'
                    OR description LIKE '%' + @buscar + '%'
                  )
            ORDER BY nombre_etiqueta;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@buscar", string.IsNullOrWhiteSpace(buscar) ? DBNull.Value : buscar.Trim());

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var productos = new List<ProductoDto>();
        while (await reader.ReadAsync(cancellationToken))
        {
            productos.Add(new ProductoDto(
                reader.GetInt32(0),
                reader.IsDBNull(1) ? null : reader.GetString(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.GetInt32(4),
                reader.GetDecimal(5),
                reader.GetBoolean(6),
                reader.GetBoolean(7)));
        }

        return productos;
    }
}
