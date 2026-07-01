using Microsoft.AspNetCore.Mvc;
using SalesPro.Business.Services;
using SalesPro.Contracts.Catalogos;

namespace SalesPro.Api.Controllers;

[ApiController]
[Route("api/catalogos")]
public sealed class CatalogosController : ControllerBase
{
    private readonly ICatalogoService _catalogoService;

    public CatalogosController(ICatalogoService catalogoService)
    {
        _catalogoService = catalogoService;
    }

    [HttpGet("bancos")]
    public async Task<ActionResult<IReadOnlyCollection<BancoDto>>> ListarBancos(CancellationToken cancellationToken)
    {
        return Ok(await _catalogoService.ListarBancosAsync(cancellationToken));
    }

    [HttpGet("companias")]
    public async Task<ActionResult<IReadOnlyCollection<CompaniaDto>>> ListarCompanias(CancellationToken cancellationToken)
    {
        return Ok(await _catalogoService.ListarCompaniasAsync(cancellationToken));
    }

    [HttpGet("clientes")]
    public async Task<ActionResult<IReadOnlyCollection<ClienteDto>>> BuscarClientes([FromQuery] string? buscar, CancellationToken cancellationToken)
    {
        return Ok(await _catalogoService.BuscarClientesAsync(buscar, cancellationToken));
    }

    [HttpGet("productos")]
    public async Task<ActionResult<IReadOnlyCollection<ProductoDto>>> BuscarProductos([FromQuery] string? buscar, CancellationToken cancellationToken)
    {
        return Ok(await _catalogoService.BuscarProductosAsync(buscar, cancellationToken));
    }
}
