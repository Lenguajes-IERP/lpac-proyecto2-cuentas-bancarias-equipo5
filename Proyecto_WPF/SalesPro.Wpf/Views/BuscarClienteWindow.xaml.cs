using System.Windows;
using SalesPro.Wpf.ViewModels;

namespace SalesPro.Wpf.Views;

public partial class BuscarClienteWindow : Window
{
    public BuscarClienteWindow()
    {
        InitializeComponent();
    }

    private void Cerrar_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Seleccionar_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is NuevaOrdenViewModel viewModel)
        {
            if (viewModel.SeleccionarClienteCommand.CanExecute(null))
            {
                viewModel.SeleccionarClienteCommand.Execute(null);
                Close();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un cliente.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
