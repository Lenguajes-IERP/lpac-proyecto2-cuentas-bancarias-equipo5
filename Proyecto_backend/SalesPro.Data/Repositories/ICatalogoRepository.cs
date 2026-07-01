using SalesPro.Contracts.Catalogos;

namespace SalesPro.Data.Repositories;

public interface ICatalogoRepository
{
    Task<IReadOnlyCollection<BancoDto>> ListarBancosAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<CompaniaDto>> ListarCompaniasAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<ClienteDto>> BuscarClientesAsync(string? buscar, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<ProductoDto>> BuscarProductosAsync(string? buscar, CancellationToken cancellationToken);
}
