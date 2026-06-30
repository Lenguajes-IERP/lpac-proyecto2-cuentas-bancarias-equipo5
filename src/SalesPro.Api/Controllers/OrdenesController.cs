using Microsoft.AspNetCore.Mvc;
using SalesPro.Business.Services;
using SalesPro.Contracts.Ordenes;

namespace SalesPro.Api.Controllers;

[ApiController]
[Route("api/ordenes")]
public sealed class OrdenesController : ControllerBase
{
    private readonly IOrdenService _ordenService;

    public OrdenesController(IOrdenService ordenService)
    {
        _ordenService = ordenService;
    }

    [HttpGet("{numeroOrden:int}")]
    public async Task<ActionResult<OrdenDto>> ObtenerPorNumero(int numeroOrden, CancellationToken cancellationToken)
    {
        return Ok(await _ordenService.ObtenerPorNumeroAsync(numeroOrden, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<OrdenDto>> Crear([FromBody] CrearOrdenRequest request, CancellationToken cancellationToken)
    {
        var created = await _ordenService.CrearOrdenAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorNumero), new { numeroOrden = created.NumeroOrden }, created);
    }
}
