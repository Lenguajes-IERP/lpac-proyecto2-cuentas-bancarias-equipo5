using Microsoft.AspNetCore.Mvc;
using SalesPro.Business.Services;
using SalesPro.Contracts.Ordenes;

namespace SalesPro.Api.Controllers;

[ApiController]
[Route("api/ordenes")]
[Produces("application/json")]
public sealed class OrdenesController : ControllerBase
{
    // El controlador no calcula ni valida reglas de negocio.
    // Solo recibe HTTP, llama al servicio y devuelve una respuesta REST.
    private readonly IOrdenService _ordenService;

    public OrdenesController(IOrdenService ordenService)
    {
        _ordenService = ordenService;
    }

    [HttpGet("{numeroOrden:int}")]
    [ProducesResponseType(typeof(OrdenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrdenDto>> ObtenerPorNumero(int numeroOrden, CancellationToken cancellationToken)
    {
        // Si no existe, el servicio lanza NotFoundException y el middleware la convierte en 404.
        return Ok(await _ordenService.ObtenerPorNumeroAsync(numeroOrden, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrdenDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<OrdenDto>> Crear([FromBody] CrearOrdenRequest request, CancellationToken cancellationToken)
    {
        var created = await _ordenService.CrearOrdenAsync(request, cancellationToken);
        // CreatedAtAction devuelve 201 y deja la ruta para consultar la orden creada.
        return CreatedAtAction(nameof(ObtenerPorNumero), new { numeroOrden = created.NumeroOrden }, created);
    }
}
