using System.Windows;
using SalesPro.Wpf.ViewModels;

namespace SalesPro.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
        ActualizarTextoTema();
    }

    private void ThemeToggleButton_Click(object sender, RoutedEventArgs e)
    {
        App.AlternarTema();
        ActualizarTextoTema();
    }

    private void ActualizarTextoTema()
    {
        ThemeToggleButton.Content = App.IsDarkTheme ? "Tema claro" : "Tema oscuro";
    }
}
