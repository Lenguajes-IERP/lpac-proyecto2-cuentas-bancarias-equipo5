using Microsoft.AspNetCore.Mvc;
using SalesPro.Business.Services;
using SalesPro.Contracts.CuentasBancarias;

namespace SalesPro.Api.Controllers;

[ApiController]
[Route("api/cuentas-bancarias")]
public sealed class CuentasBancariasController : ControllerBase
{
    private readonly ICuentaBancariaService _cuentaBancariaService;

    public CuentasBancariasController(ICuentaBancariaService cuentaBancariaService)
    {
        _cuentaBancariaService = cuentaBancariaService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CuentaBancariaDto>>> Listar([FromQuery] string? buscar, CancellationToken cancellationToken)
    {
        return Ok(await _cuentaBancariaService.ListarAsync(buscar, cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CuentaBancariaDto>> ObtenerPorId(int id, CancellationToken cancellationToken)
    {
        return Ok(await _cuentaBancariaService.ObtenerPorIdAsync(id, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<CuentaBancariaDto>> Crear([FromBody] CrearCuentaBancariaRequest request, CancellationToken cancellationToken)
    {
        var created = await _cuentaBancariaService.CrearAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CuentaBancariaDto>> Actualizar(int id, [FromBody] ActualizarCuentaBancariaRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _cuentaBancariaService.ActualizarAsync(id, request, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        await _cuentaBancariaService.EliminarAsync(id, cancellationToken);
        return NoContent();
    }
}
