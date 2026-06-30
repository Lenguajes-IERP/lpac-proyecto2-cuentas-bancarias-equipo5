using SalesPro.Wpf.Infrastructure;
using SalesPro.Wpf.Services;

namespace SalesPro.Wpf.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    private object _currentViewModel;

    public MainViewModel()
    {
        var apiClient = new ApiClientService();
        CuentasBancariasViewModel = new CuentasBancariasViewModel(apiClient);
        NuevaOrdenViewModel = new NuevaOrdenViewModel(apiClient);
        _currentViewModel = CuentasBancariasViewModel;

        MostrarCuentasCommand = new RelayCommand(() => CurrentViewModel = CuentasBancariasViewModel);
        MostrarNuevaOrdenCommand = new RelayCommand(() => CurrentViewModel = NuevaOrdenViewModel);
    }

    public CuentasBancariasViewModel CuentasBancariasViewModel { get; }
    public NuevaOrdenViewModel NuevaOrdenViewModel { get; }

    public object CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    public RelayCommand MostrarCuentasCommand { get; }
    public RelayCommand MostrarNuevaOrdenCommand { get; }
}
