namespace SalesPro.Contracts.Ordenes;

public sealed record OrdenDto(
    int NumeroOrden,
    int ClienteId,
    string ClienteNombre,
    DateTime FechaOrden,
    int? EmpleadoId,
    decimal Subtotal,
    decimal Impuesto,
    decimal Total,
    IReadOnlyCollection<OrdenDetalleDto> Detalles);
