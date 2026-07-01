using SalesPro.Contracts.Ordenes;

namespace SalesPro.Data.Repositories;

public interface IOrdenRepository
{
    Task<OrdenDto> CrearOrdenAsync(CrearOrdenRequest request, decimal porcentajeImpuestoVenta, CancellationToken cancellationToken);
    Task<OrdenDto?> ObtenerPorNumeroAsync(int numeroOrden, CancellationToken cancellationToken);
}
