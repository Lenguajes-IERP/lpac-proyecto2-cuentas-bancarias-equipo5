using System.Windows;
using SalesPro.Wpf.ViewModels;

namespace SalesPro.Wpf.Views;

public partial class BuscarProductoWindow : Window
{
    public BuscarProductoWindow()
    {
        InitializeComponent();
    }

    private void Cerrar_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Agregar_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is NuevaOrdenViewModel viewModel)
        {
            if (viewModel.AgregarProductoCommand.CanExecute(null))
            {
                viewModel.AgregarProductoCommand.Execute(null);
                Close();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un producto y especificar una cantidad válida mayor a 0.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
