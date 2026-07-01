using SalesPro.Contracts.CuentasBancarias;

namespace SalesPro.Data.Repositories;

public interface ICuentaBancariaRepository
{
    Task<IReadOnlyCollection<CuentaBancariaDto>> ListarAsync(string? buscar, CancellationToken cancellationToken);
    Task<CuentaBancariaDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);
    Task<int> CrearAsync(CrearCuentaBancariaRequest request, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(int id, ActualizarCuentaBancariaRequest request, CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExisteBancoAsync(int bancoId, CancellationToken cancellationToken);
    Task<bool> ExisteCompaniaAsync(int companiaId, CancellationToken cancellationToken);
    Task<bool> ExisteNumeroCuentaAsync(string numeroCuenta, int bancoId, int? idExcluir, CancellationToken cancellationToken);
}
