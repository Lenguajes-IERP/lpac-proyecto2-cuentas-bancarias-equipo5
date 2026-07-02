using Microsoft.AspNetCore.Mvc;
using SalesPro.Business.Services;
using SalesPro.Contracts.Ordenes;

namespace SalesPro.Api.Controllers;

[ApiController]
[Route("api/ordenes")]
[Produces("application/json")]
public sealed class OrdenesController : ControllerBase
{
    private readonly IOrdenService _ordenService;

    public OrdenesController(IOrdenService ordenService)
    {
        _ordenService = ordenService;
    }

    // Listar
    // Lista todas las órdenes existentes con sus detalles.
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<OrdenDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<OrdenDto>>> Listar(CancellationToken cancellationToken)
    {
        return Ok(await _ordenService.ListarAsync(cancellationToken));
    }

    // ObtenerPorNumero
    // Obtiene una orden por su número (404 si no existe).
    [HttpGet("{numeroOrden:int}")]
    [ProducesResponseType(typeof(OrdenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrdenDto>> ObtenerPorNumero(int numeroOrden, CancellationToken cancellationToken)
    {
        return Ok(await _ordenService.ObtenerPorNumeroAsync(numeroOrden, cancellationToken));
    }

    // Crear
    // Crea una orden de venta en una transacción: descuenta inventario y aplica el IVA.
    [HttpPost]
    [ProducesResponseType(typeof(OrdenDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<OrdenDto>> Crear([FromBody] CrearOrdenRequest request, CancellationToken cancellationToken)
    {
        var created = await _ordenService.CrearOrdenAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorNumero), new { numeroOrden = created.NumeroOrden }, created);
    }
}
