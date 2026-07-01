using System.Windows;
using SalesPro.Wpf.ViewModels;

namespace SalesPro.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
