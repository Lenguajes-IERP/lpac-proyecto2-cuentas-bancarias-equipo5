using Microsoft.Data.SqlClient;
using SalesPro.Contracts.CuentasBancarias;
using SalesPro.Data.Infrastructure;

namespace SalesPro.Data.Repositories;

public sealed class CuentaBancariaRepository : ICuentaBancariaRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CuentaBancariaRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<CuentaBancariaDto>> ListarAsync(string? buscar, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT c.id,
                   c.numero_cuenta,
                   c.tipo_cuenta,
                   c.tipo_divisa,
                   c.estado,
                   c.pais,
                   c.provincia,
                   c.fk_banco,
                   b.nombre AS banco_nombre,
                   c.fk_compania,
                   co.nombre AS compania_nombre,
                   c.nombre_dueno,
                   c.apellidos_dueno
            FROM Compania_Cuenta_Bancaria c
            INNER JOIN Banco b ON b.id = c.fk_banco
            INNER JOIN Compania co ON co.id = c.fk_compania
            WHERE @buscar IS NULL
               OR c.numero_cuenta LIKE '%' + @buscar + '%'
               OR b.nombre LIKE '%' + @buscar + '%'
               OR co.nombre LIKE '%' + @buscar + '%'
               OR c.nombre_dueno LIKE '%' + @buscar + '%'
               OR c.apellidos_dueno LIKE '%' + @buscar + '%'
            ORDER BY c.id DESC;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@buscar", string.IsNullOrWhiteSpace(buscar) ? DBNull.Value : buscar.Trim());

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var cuentas = new List<CuentaBancariaDto>();
        while (await reader.ReadAsync(cancellationToken))
        {
            cuentas.Add(MapCuenta(reader));
        }

        return cuentas;
    }

    public async Task<CuentaBancariaDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT c.id,
                   c.numero_cuenta,
                   c.tipo_cuenta,
                   c.tipo_divisa,
                   c.estado,
                   c.pais,
                   c.provincia,
                   c.fk_banco,
                   b.nombre AS banco_nombre,
                   c.fk_compania,
                   co.nombre AS compania_nombre,
                   c.nombre_dueno,
                   c.apellidos_dueno
            FROM Compania_Cuenta_Bancaria c
            INNER JOIN Banco b ON b.id = c.fk_banco
            INNER JOIN Compania co ON co.id = c.fk_compania
            WHERE c.id = @id;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? MapCuenta(reader) : null;
    }

    public async Task<int> CrearAsync(CrearCuentaBancariaRequest request, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO Compania_Cuenta_Bancaria
                (numero_cuenta, tipo_cuenta, tipo_divisa, estado, pais, provincia, fk_banco, fk_compania, nombre_dueno, apellidos_dueno)
            OUTPUT INSERTED.id
            VALUES
                (@numeroCuenta, @tipoCuenta, @tipoDivisa, @estado, @pais, @provincia, @bancoId, @companiaId, @nombreDueno, @apellidosDueno);
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AddCuentaParameters(command, request);

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result);
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarCuentaBancariaRequest request, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE Compania_Cuenta_Bancaria
            SET numero_cuenta = @numeroCuenta,
                tipo_cuenta = @tipoCuenta,
                tipo_divisa = @tipoDivisa,
                estado = @estado,
                pais = @pais,
                provincia = @provincia,
                fk_banco = @bancoId,
                fk_compania = @companiaId,
                nombre_dueno = @nombreDueno,
                apellidos_dueno = @apellidosDueno
            WHERE id = @id;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        AddCuentaParameters(command, request);

        return await command.ExecuteNonQueryAsync(cancellationToken) == 1;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken cancellationToken)
    {
        const string sql = "DELETE FROM Compania_Cuenta_Bancaria WHERE id = @id;";

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        return await command.ExecuteNonQueryAsync(cancellationToken) == 1;
    }

    public async Task<bool> ExisteBancoAsync(int bancoId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(1) FROM Banco WHERE id = @id AND active = 1;";
        return await ExistsAsync(sql, bancoId, cancellationToken);
    }

    public async Task<bool> ExisteCompaniaAsync(int companiaId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(1) FROM Compania WHERE id = @id;";
        return await ExistsAsync(sql, companiaId, cancellationToken);
    }

    public async Task<bool> ExisteNumeroCuentaAsync(string numeroCuenta, int bancoId, int? idExcluir, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT COUNT(1)
            FROM Compania_Cuenta_Bancaria
            WHERE numero_cuenta = @numeroCuenta
              AND fk_banco = @bancoId
              AND (@idExcluir IS NULL OR id <> @idExcluir);
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@numeroCuenta", numeroCuenta.Trim());
        command.Parameters.AddWithValue("@bancoId", bancoId);
        command.Parameters.AddWithValue("@idExcluir", idExcluir is null ? DBNull.Value : idExcluir.Value);

        var count = Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
        return count > 0;
    }

    private async Task<bool> ExistsAsync(string sql, int id, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        var count = Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
        return count > 0;
    }

    private static CuentaBancariaDto MapCuenta(SqlDataReader reader)
    {
        return new CuentaBancariaDto(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetBoolean(4),
            reader.GetString(5),
            reader.GetString(6),
            reader.GetInt32(7),
            reader.GetString(8),
            reader.GetInt32(9),
            reader.GetString(10),
            reader.GetString(11),
            reader.GetString(12));
    }

    private static void AddCuentaParameters(SqlCommand command, CrearCuentaBancariaRequest request)
    {
        command.Parameters.AddWithValue("@numeroCuenta", request.NumeroCuenta.Trim());
        command.Parameters.AddWithValue("@tipoCuenta", request.TipoCuenta.Trim());
        command.Parameters.AddWithValue("@tipoDivisa", request.TipoDivisa.Trim().ToUpperInvariant());
        command.Parameters.AddWithValue("@estado", request.Estado);
        command.Parameters.AddWithValue("@pais", request.Pais.Trim());
        command.Parameters.AddWithValue("@provincia", request.Provincia.Trim());
        command.Parameters.AddWithValue("@bancoId", request.BancoId);
        command.Parameters.AddWithValue("@companiaId", request.CompaniaId);
        command.Parameters.AddWithValue("@nombreDueno", request.NombreDueno.Trim());
        command.Parameters.AddWithValue("@apellidosDueno", request.ApellidosDueno.Trim());
    }

    private static void AddCuentaParameters(SqlCommand command, ActualizarCuentaBancariaRequest request)
    {
        command.Parameters.AddWithValue("@numeroCuenta", request.NumeroCuenta.Trim());
        command.Parameters.AddWithValue("@tipoCuenta", request.TipoCuenta.Trim());
        command.Parameters.AddWithValue("@tipoDivisa", request.TipoDivisa.Trim().ToUpperInvariant());
        command.Parameters.AddWithValue("@estado", request.Estado);
        command.Parameters.AddWithValue("@pais", request.Pais.Trim());
        command.Parameters.AddWithValue("@provincia", request.Provincia.Trim());
        command.Parameters.AddWithValue("@bancoId", request.BancoId);
        command.Parameters.AddWithValue("@companiaId", request.CompaniaId);
        command.Parameters.AddWithValue("@nombreDueno", request.NombreDueno.Trim());
        command.Parameters.AddWithValue("@apellidosDueno", request.ApellidosDueno.Trim());
    }
}
