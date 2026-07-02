using SalesPro.Contracts.Ordenes;
using SalesPro.Data.Repositories;
using SalesPro.Domain.Exceptions;

namespace SalesPro.Business.Services;

public sealed class OrdenService : IOrdenService
{
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
        ValidarSolicitud(request);

        var porcentajeImpuestoVenta = await _parametroSistemaRepository.ObtenerValorDecimalAsync(NombreParametroIva, cancellationToken);
        if (porcentajeImpuestoVenta is null)
        {
            throw new NotFoundException("No se encontró el parámetro de sistema 'IVA'.");
        }

        return await _ordenRepository.CrearOrdenAsync(request, porcentajeImpuestoVenta.Value, cancellationToken);
    }

    public async Task<OrdenDto> ObtenerPorNumeroAsync(int numeroOrden, CancellationToken cancellationToken)
    {
        return await _ordenRepository.ObtenerPorNumeroAsync(numeroOrden, cancellationToken)
               ?? throw new NotFoundException($"No existe la orden {numeroOrden}.");
    }

    public Task<IReadOnlyCollection<OrdenDto>> ListarAsync(CancellationToken cancellationToken)
    {
        return _ordenRepository.ListarAsync(cancellationToken);
    }

    private static void ValidarSolicitud(CrearOrdenRequest request)
    {
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
            throw new ValidationFailureException($"La solicitud trae productos repetidos: {string.Join(", ", repetidos)}. Incremente la cantidad en una sola línea.");
        }
    }
}
