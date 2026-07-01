namespace SalesPro.Domain.Entities;

public sealed class PosOrden
{
    public int NumeroOrden { get; set; }
    public int ClienteId { get; set; }
    public DateTime FechaOrden { get; set; }
    public int? EmpleadoId { get; set; }
    public decimal TotalOrden { get; set; }
    public decimal Impuesto { get; set; }
    public List<PosOrdenDetalle> Detalles { get; set; } = [];
}
