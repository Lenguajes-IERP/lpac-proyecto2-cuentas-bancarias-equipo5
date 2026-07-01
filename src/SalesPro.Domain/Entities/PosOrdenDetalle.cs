namespace SalesPro.Domain.Entities;

public sealed class PosOrdenDetalle
{
    public int OrdenId { get; set; }
    public int ProductoId { get; set; }
    public decimal Impuesto { get; set; }
    public int Cantidad { get; set; }
    public decimal Descuento { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal PrecioSubtotal { get; set; }
}
