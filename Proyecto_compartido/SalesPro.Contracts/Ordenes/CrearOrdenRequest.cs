namespace SalesPro.Contracts.Ordenes;

public sealed record CrearOrdenRequest(
    int ClienteId,
    int? EmpleadoId,
    IReadOnlyCollection<CrearOrdenDetalleRequest> Detalles);
