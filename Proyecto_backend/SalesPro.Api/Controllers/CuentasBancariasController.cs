using Microsoft.AspNetCore.Mvc;
using SalesPro.Business.Services;
using SalesPro.Contracts.CuentasBancarias;

namespace SalesPro.Api.Controllers;

[ApiController]
[Route("api/cuentas-bancarias")]
[Produces("application/json")]
public sealed class CuentasBancariasController : ControllerBase
{
    // CRUD REST del actualizador asignado al Equipo 5: cuentas bancarias.
    private readonly ICuentaBancariaService _cuentaBancariaService;

    public CuentasBancariasController(ICuentaBancariaService cuentaBancariaService)
    {
        _cuentaBancariaService = cuentaBancariaService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CuentaBancariaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<CuentaBancariaDto>>> Listar([FromQuery] string? buscar, CancellationToken cancellationToken)
    {
        // El filtro "buscar" es opcional y se manda a datos para filtrar por cuenta, banco, compañía o dueño.
        return Ok(await _cuentaBancariaService.ListarAsync(buscar, cancellationToken));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CuentaBancariaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CuentaBancariaDto>> ObtenerPorId(int id, CancellationToken cancellationToken)
    {
        return Ok(await _cuentaBancariaService.ObtenerPorIdAsync(id, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CuentaBancariaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CuentaBancariaDto>> Crear([FromBody] CrearCuentaBancariaRequest request, CancellationToken cancellationToken)
    {
        var created = await _cuentaBancariaService.CrearAsync(request, cancellationToken);
        // 201 Created es lo correcto cuando el POST sí crea un recurso nuevo.
        return CreatedAtAction(nameof(ObtenerPorId), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CuentaBancariaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CuentaBancariaDto>> Actualizar(int id, [FromBody] ActualizarCuentaBancariaRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _cuentaBancariaService.ActualizarAsync(id, request, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        await _cuentaBancariaService.EliminarAsync(id, cancellationToken);
        // NoContent evita devolver basura cuando el DELETE fue correcto.
        return NoContent();
    }
}
