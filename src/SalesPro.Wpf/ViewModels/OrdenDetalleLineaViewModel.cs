using SalesPro.Wpf.Infrastructure;

namespace SalesPro.Wpf.ViewModels;

public sealed class OrdenDetalleLineaViewModel : ViewModelBase
{
    private int _cantidad;

    public OrdenDetalleLineaViewModel(int productoId, string nombreProducto, decimal precioUnitario, int cantidad, bool tieneImpuesto)
    {
        ProductoId = productoId;
        NombreProducto = nombreProducto;
        PrecioUnitario = precioUnitario;
        TieneImpuesto = tieneImpuesto;
        _cantidad = cantidad;
    }

    public int ProductoId { get; }
    public string NombreProducto { get; }
    public decimal PrecioUnitario { get; }
    public bool TieneImpuesto { get; }

    public int Cantidad
    {
        get => _cantidad;
        set
        {
            if (SetProperty(ref _cantidad, value))
            {
                OnPropertyChanged(nameof(Subtotal));
                OnPropertyChanged(nameof(ImpuestoEstimado));
                OnPropertyChanged(nameof(TotalEstimado));
            }
        }
    }

    public decimal Subtotal => PrecioUnitario * Cantidad;
    public decimal ImpuestoEstimado => TieneImpuesto ? Math.Round(Subtotal * 0.13m, 2, MidpointRounding.AwayFromZero) : 0m;
    public decimal TotalEstimado => Subtotal + ImpuestoEstimado;
}
