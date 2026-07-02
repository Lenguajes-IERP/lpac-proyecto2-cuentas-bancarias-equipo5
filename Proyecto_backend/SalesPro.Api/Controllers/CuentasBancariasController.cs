using Microsoft.AspNetCore.Mvc;
using SalesPro.Business.Services;
using SalesPro.Contracts.CuentasBancarias;

namespace SalesPro.Api.Controllers;

[ApiController]
[Route("api/cuentas-bancarias")]
[Produces("application/json")]
public sealed class CuentasBancariasController : ControllerBase
{
    private readonly ICuentaBancariaService _cuentaBancariaService;

    public CuentasBancariasController(ICuentaBancariaService cuentaBancariaService)
    {
        _cuentaBancariaService = cuentaBancariaService;
    }


    // Listar
    // Lista todas las cuentas bancarias (filtro opcional por texto de búsqueda).
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CuentaBancariaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<CuentaBancariaDto>>> Listar([FromQuery] string? buscar, CancellationToken cancellationToken)
    {
        return Ok(await _cuentaBancariaService.ListarAsync(buscar, cancellationToken));
    }

    // ObtenerPorId
    // Obtiene una cuenta bancaria por su id (404 si no existe).
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CuentaBancariaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CuentaBancariaDto>> ObtenerPorId(int id, CancellationToken cancellationToken)
    {
        return Ok(await _cuentaBancariaService.ObtenerPorIdAsync(id, cancellationToken));
    }

    // Crear
    // Crea una nueva cuenta bancaria (400 validación, 409 número duplicado).
    [HttpPost]
    [ProducesResponseType(typeof(CuentaBancariaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CuentaBancariaDto>> Crear([FromBody] CrearCuentaBancariaRequest request, CancellationToken cancellationToken)
    {
        var created = await _cuentaBancariaService.CrearAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = created.Id }, created);
    }

    // Actualizar
    // Actualiza los datos de una cuenta bancaria existente.
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CuentaBancariaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CuentaBancariaDto>> Actualizar(int id, [FromBody] ActualizarCuentaBancariaRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _cuentaBancariaService.ActualizarAsync(id, request, cancellationToken));
    }

    // Eliminar
    // Elimina una cuenta bancaria por su id (204 si se elimina, 404 si no existe).
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        await _cuentaBancariaService.EliminarAsync(id, cancellationToken);
        return NoContent();
    }
}
