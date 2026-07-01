using SalesPro.Contracts.Catalogos;
using SalesPro.Data.Repositories;

namespace SalesPro.Business.Services;

public sealed class CatalogoService : ICatalogoService
{
    private readonly ICatalogoRepository _catalogoRepository;

    public CatalogoService(ICatalogoRepository catalogoRepository)
    {
        _catalogoRepository = catalogoRepository;
    }

    public Task<IReadOnlyCollection<BancoDto>> ListarBancosAsync(CancellationToken cancellationToken)
    {
        return _catalogoRepository.ListarBancosAsync(cancellationToken);
    }

    public Task<IReadOnlyCollection<CompaniaDto>> ListarCompaniasAsync(CancellationToken cancellationToken)
    {
        return _catalogoRepository.ListarCompaniasAsync(cancellationToken);
    }

    public Task<IReadOnlyCollection<ClienteDto>> BuscarClientesAsync(string? buscar, CancellationToken cancellationToken)
    {
        return _catalogoRepository.BuscarClientesAsync(buscar, cancellationToken);
    }

    public Task<IReadOnlyCollection<ProductoDto>> BuscarProductosAsync(string? buscar, CancellationToken cancellationToken)
    {
        return _catalogoRepository.BuscarProductosAsync(buscar, cancellationToken);
    }
}
