using SalesPro.Contracts.CuentasBancarias;

namespace SalesPro.Business.Services;

public interface ICuentaBancariaService
{
    Task<IReadOnlyCollection<CuentaBancariaDto>> ListarAsync(string? buscar, CancellationToken cancellationToken);
    Task<CuentaBancariaDto> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);
    Task<CuentaBancariaDto> CrearAsync(CrearCuentaBancariaRequest request, CancellationToken cancellationToken);
    Task<CuentaBancariaDto> ActualizarAsync(int id, ActualizarCuentaBancariaRequest request, CancellationToken cancellationToken);
    Task EliminarAsync(int id, CancellationToken cancellationToken);
}
