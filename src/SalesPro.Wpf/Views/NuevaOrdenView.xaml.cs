using System.Windows.Controls;
using System.Windows;

namespace SalesPro.Wpf.Views;

public partial class NuevaOrdenView : UserControl
{
    public NuevaOrdenView()
    {
        InitializeComponent();
    }

    private void AbrirBusqueda_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var window = new BuscarProductoWindow
        {
            Owner = Window.GetWindow(this),
            DataContext = DataContext
        };
        window.ShowDialog();
    }

    private void AbrirBusquedaCliente_Click(object sender, RoutedEventArgs e)
    {
        var window = new BuscarClienteWindow
        {
            Owner = Window.GetWindow(this),
            DataContext = DataContext
        };
        window.ShowDialog();
    }
}
