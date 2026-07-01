using System.Windows.Controls;

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
            DataContext = this.DataContext
        };
        window.ShowDialog();
    }
}
