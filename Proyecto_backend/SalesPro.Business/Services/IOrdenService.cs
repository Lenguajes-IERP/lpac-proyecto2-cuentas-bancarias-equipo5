using SalesPro.Contracts.Ordenes;

namespace SalesPro.Business.Services;

public interface IOrdenService
{
    Task<OrdenDto> CrearOrdenAsync(CrearOrdenRequest request, CancellationToken cancellationToken);
    Task<OrdenDto> ObtenerPorNumeroAsync(int numeroOrden, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<OrdenDto>> ListarAsync(CancellationToken cancellationToken);
}
