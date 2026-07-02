using System.Collections.ObjectModel;
using SalesPro.Contracts.Catalogos;
using SalesPro.Contracts.CuentasBancarias;
using SalesPro.Wpf.Infrastructure;
using SalesPro.Wpf.Models;
using SalesPro.Wpf.Services;

namespace SalesPro.Wpf.ViewModels;

public sealed class CuentasBancariasViewModel : ViewModelBase
{
    // ViewModel del CRUD asignado al equipo: cuentas bancarias.
    // Maneja listas, formulario y comandos sin poner lógica en el code-behind de la vista.
    private readonly ApiClientService _apiClient;
    private string _buscar = string.Empty;
    private string _mensaje = string.Empty;
    private CuentaBancariaDto? _cuentaSeleccionada;
    private int? _idEnEdicion;
    private CuentaBancariaFormModel _form = new();

    public CuentasBancariasViewModel(ApiClientService apiClient)
    {
        _apiClient = apiClient;

        // Cada comando corresponde a un botón o acción de la pantalla.
        CargarCommand = new AsyncRelayCommand(CargarAsync);
        NuevaCommand = new RelayCommand(Nueva);
        EditarCommand = new RelayCommand(Editar, () => CuentaSeleccionada is not null);
        GuardarCommand = new AsyncRelayCommand(GuardarAsync);
        EliminarCommand = new AsyncRelayCommand(EliminarAsync, () => CuentaSeleccionada is not null);
        CancelarCommand = new RelayCommand(Nueva);

        // Se cargan catálogos y cuentas apenas se abre la pantalla.
        _ = CargarAsync();
    }

    public ObservableCollection<CuentaBancariaDto> Cuentas { get; } = [];
    public ObservableCollection<BancoDto> Bancos { get; } = [];
    public ObservableCollection<CompaniaDto> Companias { get; } = [];

    public IReadOnlyCollection<string> TiposCuenta { get; } = ["Corriente", "Ahorro", "Planilla"];
    public IReadOnlyCollection<string> TiposDivisa { get; } = ["CRC", "USD", "EUR"];

    public string Buscar
    {
        get => _buscar;
        set => SetProperty(ref _buscar, value);
    }

    public string Mensaje
    {
        get => _mensaje;
        set => SetProperty(ref _mensaje, value);
    }

    public CuentaBancariaDto? CuentaSeleccionada
    {
        get => _cuentaSeleccionada;
        set
        {
            if (SetProperty(ref _cuentaSeleccionada, value))
            {
                EditarCommand.RaiseCanExecuteChanged();
                EliminarCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public CuentaBancariaFormModel Form
    {
        get => _form;
        set => SetProperty(ref _form, value);
    }

    public string TituloFormulario => _idEnEdicion is null ? "Nueva cuenta bancaria" : $"Editando cuenta #{_idEnEdicion}";

    public AsyncRelayCommand CargarCommand { get; }
    public RelayCommand NuevaCommand { get; }
    public RelayCommand EditarCommand { get; }
    public AsyncRelayCommand GuardarCommand { get; }
    public AsyncRelayCommand EliminarCommand { get; }
    public RelayCommand CancelarCommand { get; }

    private async Task CargarAsync()
    {
        try
        {
            Mensaje = "Cargando datos...";

            // La vista carga catálogos y cuentas desde la API; no hay conexión directa a SQL desde WPF.
            var bancos = await _apiClient.GetAsync<IReadOnlyCollection<BancoDto>>("api/catalogos/bancos");
            var companias = await _apiClient.GetAsync<IReadOnlyCollection<CompaniaDto>>("api/catalogos/companias");
            var cuentas = await _apiClient.GetAsync<IReadOnlyCollection<CuentaBancariaDto>>($"api/cuentas-bancarias?buscar={Uri.EscapeDataString(Buscar)}");

            Replace(Bancos, bancos);
            Replace(Companias, companias);
            Replace(Cuentas, cuentas);

            if (Bancos.Count > 0 && Form.BancoId <= 0)
            {
                Form.BancoId = Bancos[0].Id;
            }

            if (Companias.Count > 0 && Form.CompaniaId <= 0)
            {
                Form.CompaniaId = Companias[0].Id;
            }

            Mensaje = "Datos cargados.";
        }
        catch (Exception ex)
        {
            Mensaje = ex.Message;
        }
    }

    private void Nueva()
    {
        // Reinicia el formulario para crear, usando valores por defecto de los catálogos.
        _idEnEdicion = null;
        Form = new CuentaBancariaFormModel
        {
            BancoId = Bancos.FirstOrDefault()?.Id ?? 1,
            CompaniaId = Companias.FirstOrDefault()?.Id ?? 1
        };
        OnPropertyChanged(nameof(TituloFormulario));
        Mensaje = "Formulario listo para crear.";
    }

    private void Editar()
    {
        if (CuentaSeleccionada is null)
        {
            return;
        }

        // Se copia la cuenta seleccionada al formulario.
        // No se edita el DTO directamente para evitar cambios visuales antes de guardar.
        _idEnEdicion = CuentaSeleccionada.Id;
        Form = new CuentaBancariaFormModel
        {
            NumeroCuenta = CuentaSeleccionada.NumeroCuenta,
            TipoCuenta = CuentaSeleccionada.TipoCuenta,
            TipoDivisa = CuentaSeleccionada.TipoDivisa,
            Estado = CuentaSeleccionada.Estado,
            Pais = CuentaSeleccionada.Pais,
            Provincia = CuentaSeleccionada.Provincia,
            BancoId = CuentaSeleccionada.BancoId,
            CompaniaId = CuentaSeleccionada.CompaniaId,
            NombreDueno = CuentaSeleccionada.NombreDueno,
            ApellidosDueno = CuentaSeleccionada.ApellidosDueno
        };
        OnPropertyChanged(nameof(TituloFormulario));
        Mensaje = "Editando copia de la cuenta seleccionada.";
    }

    private async Task GuardarAsync()
    {
        try
        {
            if (_idEnEdicion is null)
            {
                // Sin id en edición significa POST: crear una cuenta nueva.
                var request = new CrearCuentaBancariaRequest(
                    Form.NumeroCuenta,
                    Form.TipoCuenta,
                    Form.TipoDivisa,
                    Form.Estado,
                    Form.Pais,
                    Form.Provincia,
                    Form.BancoId,
                    Form.CompaniaId,
                    Form.NombreDueno,
                    Form.ApellidosDueno);

                await _apiClient.PostAsync<CrearCuentaBancariaRequest, CuentaBancariaDto>("api/cuentas-bancarias", request);
                Mensaje = "Cuenta creada correctamente.";
            }
            else
            {
                // Con id en edición significa PUT: actualizar la cuenta existente.
                var request = new ActualizarCuentaBancariaRequest(
                    Form.NumeroCuenta,
                    Form.TipoCuenta,
                    Form.TipoDivisa,
                    Form.Estado,
                    Form.Pais,
                    Form.Provincia,
                    Form.BancoId,
                    Form.CompaniaId,
                    Form.NombreDueno,
                    Form.ApellidosDueno);

                await _apiClient.PutAsync<ActualizarCuentaBancariaRequest, CuentaBancariaDto>($"api/cuentas-bancarias/{_idEnEdicion}", request);
                Mensaje = "Cuenta actualizada correctamente.";
            }

            await CargarAsync();
            Nueva();
        }
        catch (Exception ex)
        {
            Mensaje = ex.Message;
        }
    }

    private async Task EliminarAsync()
    {
        if (CuentaSeleccionada is null)
        {
            return;
        }

        // Se pide confirmación porque el DELETE es físico en base de datos.
        var result = System.Windows.MessageBox.Show(
            $"¿Está seguro que desea eliminar la cuenta {CuentaSeleccionada.NumeroCuenta} físicamente?",
            "Confirmar eliminación",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning);

        if (result != System.Windows.MessageBoxResult.Yes)
        {
            return;
        }

        try
        {
            await _apiClient.DeleteAsync($"api/cuentas-bancarias/{CuentaSeleccionada.Id}");
            Mensaje = "Cuenta eliminada físicamente.";
            await CargarAsync();
            Nueva();
        }
        catch (Exception ex)
        {
            Mensaje = ex.Message;
        }
    }

    private static void Replace<T>(ObservableCollection<T> collection, IEnumerable<T> values)
    {
        // WPF escucha la colección; por eso se limpia y se vuelve a llenar en vez de reemplazar la instancia.
        collection.Clear();
        foreach (var value in values)
        {
            collection.Add(value);
        }
    }
}
