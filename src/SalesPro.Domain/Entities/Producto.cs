namespace SalesPro.Domain.Entities;

public sealed class Producto
{
    public int ProductoId { get; set; }
    public string NombreEtiqueta { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ExistenciaEnStock { get; set; }
    public decimal PrecioNeto { get; set; }
    public bool PuedeVenderse { get; set; } = true;
    public bool TieneImpuesto { get; set; } = true;
    public string? CodigoBarra { get; set; }
}
