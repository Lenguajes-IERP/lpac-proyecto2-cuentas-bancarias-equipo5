using SalesPro.Domain.Exceptions;

namespace SalesPro.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    // Middleware centralizado: todos los controladores pueden lanzar excepciones del dominio
    // y aquí se traducen a códigos HTTP consistentes.
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SalesProException ex)
        {
            // Errores esperados del negocio: validación, no encontrado, conflicto, etc.
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new
            {
                status = ex.StatusCode,
                code = ex.ErrorCode,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            // Errores no esperados: se registra detalle en logs, pero al cliente se le da un mensaje general.
            _logger.LogError(ex, "Error no controlado procesando la solicitud.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                status = 500,
                code = "unexpected_error",
                message = "Ocurrió un error inesperado."
            });
        }
    }
}
