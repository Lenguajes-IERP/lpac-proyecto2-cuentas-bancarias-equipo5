namespace SalesPro.Contracts.Ordenes;

public sealed record CrearOrdenDetalleRequest(
    int ProductoId,
    int Cantidad);
