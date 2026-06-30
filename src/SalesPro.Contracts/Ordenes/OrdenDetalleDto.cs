namespace SalesPro.Contracts.Ordenes;

public sealed record OrdenDetalleDto(
    int ProductoId,
    string NombreProducto,
    decimal PrecioUnitario,
    int Cantidad,
    decimal Subtotal,
    decimal Impuesto);
