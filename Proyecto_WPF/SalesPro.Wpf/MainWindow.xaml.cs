using System.Windows;
using SalesPro.Wpf.Services;
using SalesPro.Wpf.ViewModels;

namespace SalesPro.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    private void TemaClaro_Click(object sender, RoutedEventArgs e)
    {
        TemaService.Aplicar(Tema.Claro);
    }

    private void TemaAltoContraste_Click(object sender, RoutedEventArgs e)
    {
        TemaService.Aplicar(Tema.AltoContraste);
    }
}
