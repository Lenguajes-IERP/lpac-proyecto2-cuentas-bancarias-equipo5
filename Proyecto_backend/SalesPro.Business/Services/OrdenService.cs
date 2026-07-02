using SalesPro.Contracts.Ordenes;
using SalesPro.Data.Repositories;
using SalesPro.Domain.Exceptions;

namespace SalesPro.Business.Services;

public sealed class OrdenService : IOrdenService
{
    // El nombre del parámetro se centraliza para no quemar "IVA" en varias partes.
    private const string NombreParametroIva = "IVA";

    private readonly IOrdenRepository _ordenRepository;
    private readonly IParametroSistemaRepository _parametroSistemaRepository;

    public OrdenService(IOrdenRepository ordenRepository, IParametroSistemaRepository parametroSistemaRepository)
    {
        _ordenRepository = ordenRepository;
        _parametroSistemaRepository = parametroSistemaRepository;
    }

    public async Task<OrdenDto> CrearOrdenAsync(CrearOrdenRequest request, CancellationToken cancellationToken)
    {
        // La capa de negocio valida la solicitud antes de llegar a SQL.
        // La capa de datos vuelve a validar existencia/stock porque eso depende de la base.
        ValidarSolicitud(request);

        // El IVA se lee desde ParametroSistema para no dejar el porcentaje fijo en código.
        var porcentajeImpuestoVenta = await _parametroSistemaRepository.ObtenerValorDecimalAsync(NombreParametroIva, cancellationToken);
        if (porcentajeImpuestoVenta is null)
        {
            throw new NotFoundException("No se encontró el parámetro de sistema 'IVA'.");
        }

        return await _ordenRepository.CrearOrdenAsync(request, porcentajeImpuestoVenta.Value, cancellationToken);
    }

    public async Task<OrdenDto> ObtenerPorNumeroAsync(int numeroOrden, CancellationToken cancellationToken)
    {
        // El repositorio devuelve null si no existe; negocio lo convierte en excepción controlada.
        return await _ordenRepository.ObtenerPorNumeroAsync(numeroOrden, cancellationToken)
               ?? throw new NotFoundException($"No existe la orden {numeroOrden}.");
    }

    private static void ValidarSolicitud(CrearOrdenRequest request)
    {
        // Estas validaciones evitan llamadas innecesarias a base de datos y producen errores claros para WPF/API.
        if (request.ClienteId <= 0)
        {
            throw new ValidationFailureException("Debe seleccionar un cliente válido.");
        }

        if (request.EmpleadoId is <= 0)
        {
            throw new ValidationFailureException("El empleado seleccionado no es válido.");
        }

        if (request.Detalles.Count == 0)
        {
            throw new ValidationFailureException("La orden debe incluir al menos un producto.");
        }

        foreach (var detalle in request.Detalles)
        {
            if (detalle.ProductoId <= 0)
            {
                throw new ValidationFailureException("Cada detalle debe tener un producto válido.");
            }

            if (detalle.Cantidad <= 0)
            {
                throw new ValidationFailureException("La cantidad de cada producto debe ser mayor que cero.");
            }
        }

        var repetidos = request.Detalles
            .GroupBy(d => d.ProductoId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToArray();

        if (repetidos.Length > 0)
        {
            // El frontend ya agrupa cantidades, pero se valida aquí para proteger la API si alguien llama directo.
            throw new ValidationFailureException($"La solicitud trae productos repetidos: {string.Join(", ", repetidos)}. Incremente la cantidad en una sola línea.");
        }
    }
}
