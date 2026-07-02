using Microsoft.AspNetCore.Mvc;
using SalesPro.Business.Services;
using SalesPro.Contracts.Catalogos;

namespace SalesPro.Api.Controllers;

[ApiController]
[Route("api/catalogos")]
[Produces("application/json")]
public sealed class CatalogosController : ControllerBase
{
    private readonly ICatalogoService _catalogoService;

    public CatalogosController(ICatalogoService catalogoService)
    {
        _catalogoService = catalogoService;
    }

    // ListarBancos
    // Lista los bancos registrados y activos.
    [HttpGet("bancos")]
    [ProducesResponseType(typeof(IReadOnlyCollection<BancoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<BancoDto>>> ListarBancos(CancellationToken cancellationToken)
    {
        return Ok(await _catalogoService.ListarBancosAsync(cancellationToken));
    }

    // ListarCompanias
    // Lista las compañías registradas.
    [HttpGet("companias")]
    [ProducesResponseType(typeof(IReadOnlyCollection<CompaniaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<CompaniaDto>>> ListarCompanias(CancellationToken cancellationToken)
    {
        return Ok(await _catalogoService.ListarCompaniasAsync(cancellationToken));
    }

    // BuscarClientes
    // Busca clientes por nombre, apellidos o identificación (filtro opcional).
    [HttpGet("clientes")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ClienteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ClienteDto>>> BuscarClientes([FromQuery] string? buscar, CancellationToken cancellationToken)
    {
        return Ok(await _catalogoService.BuscarClientesAsync(buscar, cancellationToken));
    }

    // BuscarProductos
    // Busca productos por nombre o código (filtro opcional).
    [HttpGet("productos")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ProductoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ProductoDto>>> BuscarProductos([FromQuery] string? buscar, CancellationToken cancellationToken)
    {
        return Ok(await _catalogoService.BuscarProductosAsync(buscar, cancellationToken));
    }
}
