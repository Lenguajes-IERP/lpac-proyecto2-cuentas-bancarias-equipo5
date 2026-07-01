using Microsoft.Data.SqlClient;
using SalesPro.Data.Infrastructure;

namespace SalesPro.Data.Repositories;

public sealed class ParametroSistemaRepository : IParametroSistemaRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ParametroSistemaRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<decimal?> ObtenerValorDecimalAsync(string nombre, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT valor_decimal
            FROM ParametroSistema
            WHERE nombre = @nombre;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@nombre", nombre);

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is null || result == DBNull.Value ? null : Convert.ToDecimal(result);
    }
}
