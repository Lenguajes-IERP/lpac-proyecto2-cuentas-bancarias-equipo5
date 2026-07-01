using System.Collections.ObjectModel;
using SalesPro.Contracts.Catalogos;
using SalesPro.Contracts.Ordenes;
using SalesPro.Wpf.Infrastructure;
using SalesPro.Wpf.Services;

namespace SalesPro.Wpf.ViewModels;

public sealed class NuevaOrdenViewModel : ViewModelBase
{
    private readonly ApiClientService _apiClient;
    private string _mensaje = string.Empty;
    private string _clienteBusqueda = string.Empty;
    private string _productoBusqueda = string.Empty;
    private int _clienteId;
    private int? _empleadoId = 1;
    private ClienteDto? _clienteSeleccionado;
    private ProductoDto? _productoSeleccionado;
    private OrdenDetalleLineaViewModel? _detalleSeleccionado;
    private int _cantidadProducto = 1;

    public NuevaOrdenViewModel(ApiClientService apiClient)
    {
        _apiClient = apiClient;

        BuscarClientesCommand = new AsyncRelayCommand(BuscarClientesAsync);
        SeleccionarClienteCommand = new RelayCommand(SeleccionarCliente, () => ClienteSeleccionado is not null);
        BuscarProductosCommand = new AsyncRelayCommand(BuscarProductosAsync);
        AgregarProductoCommand = new RelayCommand(AgregarProducto, () => ProductoSeleccionado is not null && CantidadProducto > 0);
        IncrementarCommand = new RelayCommand(Incrementar, () => DetalleSeleccionado is not null);
        DecrementarCommand = new RelayCommand(Decrementar, () => DetalleSeleccionado is not null);
        RemoverProductoCommand = new RelayCommand(RemoverProducto, () => DetalleSeleccionado is not null);
        ProcesarOrdenCommand = new AsyncRelayCommand(ProcesarOrdenAsync, () => Detalles.Count > 0 && ClienteId > 0);
        LimpiarCommand = new RelayCommand(Limpiar);
    }

    public ObservableCollection<ClienteDto> ClientesEncontrados { get; } = [];
    public ObservableCollection<ProductoDto> ProductosEncontrados { get; } = [];
    public ObservableCollection<OrdenDetalleLineaViewModel> Detalles { get; } = [];

    public string Mensaje
    {
        get => _mensaje;
        set => SetProperty(ref _mensaje, value);
    }

    public int ClienteId
    {
        get => _clienteId;
        private set
        {
            if (SetProperty(ref _clienteId, value))
            {
                ProcesarOrdenCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(ClienteResumen));
            }
        }
    }

    public string ClienteBusqueda
    {
        get => _clienteBusqueda;
        set => SetProperty(ref _clienteBusqueda, value);
    }

    public ClienteDto? ClienteSeleccionado
    {
        get => _clienteSeleccionado;
        set
        {
            if (SetProperty(ref _clienteSeleccionado, value))
            {
                SeleccionarClienteCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(ClienteResumen));
            }
        }
    }

    public string ClienteResumen => ClienteId <= 0 || ClienteSeleccionado is null
        ? "Sin cliente seleccionado"
        : $"{ClienteSeleccionado.Nombre} {ClienteSeleccionado.Apellidos} - {ClienteSeleccionado.NumeroIdentificacion}";

    public int? EmpleadoId
    {
        get => _empleadoId;
        set => SetProperty(ref _empleadoId, value);
    }

    public string ProductoBusqueda
    {
        get => _productoBusqueda;
        set => SetProperty(ref _productoBusqueda, value);
    }

    public ProductoDto? ProductoSeleccionado
    {
        get => _productoSeleccionado;
        set
        {
            if (SetProperty(ref _productoSeleccionado, value))
            {
                AgregarProductoCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public OrdenDetalleLineaViewModel? DetalleSeleccionado
    {
        get => _detalleSeleccionado;
        set
        {
            if (SetProperty(ref _detalleSeleccionado, value))
            {
                IncrementarCommand.RaiseCanExecuteChanged();
                DecrementarCommand.RaiseCanExecuteChanged();
                RemoverProductoCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public int CantidadProducto
    {
        get => _cantidadProducto;
        set
        {
            if (SetProperty(ref _cantidadProducto, value))
            {
                AgregarProductoCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public decimal Subtotal => Detalles.Sum(d => d.Subtotal);
    public decimal ImpuestoEstimado => Detalles.Sum(d => d.ImpuestoEstimado);
    public decimal TotalEstimado => Detalles.Sum(d => d.TotalEstimado);

    public AsyncRelayCommand BuscarClientesCommand { get; }
    public RelayCommand SeleccionarClienteCommand { get; }
    public AsyncRelayCommand BuscarProductosCommand { get; }
    public RelayCommand AgregarProductoCommand { get; }
    public RelayCommand IncrementarCommand { get; }
    public RelayCommand DecrementarCommand { get; }
    public RelayCommand RemoverProductoCommand { get; }
    public AsyncRelayCommand ProcesarOrdenCommand { get; }
    public RelayCommand LimpiarCommand { get; }

    private async Task BuscarClientesAsync()
    {
        try
        {
            var clientes = await _apiClient.GetAsync<IReadOnlyCollection<ClienteDto>>($"api/catalogos/clientes?buscar={Uri.EscapeDataString(ClienteBusqueda)}");
            ClientesEncontrados.Clear();
            foreach (var cliente in clientes)
            {
                ClientesEncontrados.Add(cliente);
            }

            Mensaje = clientes.Count == 0 ? "No se encontraron clientes." : "Clientes cargados.";
        }
        catch (Exception ex)
        {
            Mensaje = ex.Message;
        }
    }

    private void SeleccionarCliente()
    {
        if (ClienteSeleccionado is null)
        {
            return;
        }

        ClienteId = ClienteSeleccionado.Id;
        Mensaje = $"Cliente seleccionado: {ClienteResumen}.";
    }

    private async Task BuscarProductosAsync()
    {
        try
        {
            var productos = await _apiClient.GetAsync<IReadOnlyCollection<ProductoDto>>($"api/catalogos/productos?buscar={Uri.EscapeDataString(ProductoBusqueda)}");
            ProductosEncontrados.Clear();
            foreach (var producto in productos)
            {
                ProductosEncontrados.Add(producto);
            }

            Mensaje = productos.Count == 0 ? "No se encontraron productos." : "Productos cargados.";
        }
        catch (Exception ex)
        {
            Mensaje = ex.Message;
        }
    }

    private void AgregarProducto()
    {
        if (ProductoSeleccionado is null || CantidadProducto <= 0)
        {
            return;
        }

        var existente = Detalles.FirstOrDefault(d => d.ProductoId == ProductoSeleccionado.ProductoId);
        if (existente is not null)
        {
            existente.Cantidad += CantidadProducto;
        }
        else
        {
            Detalles.Add(new OrdenDetalleLineaViewModel(
                ProductoSeleccionado.ProductoId,
                ProductoSeleccionado.NombreEtiqueta,
                ProductoSeleccionado.PrecioNeto,
                CantidadProducto,
                ProductoSeleccionado.TieneImpuesto));
        }

        ProductoSeleccionado = null;
        CantidadProducto = 1;
        RefrescarTotales();
        Mensaje = "Producto agregado a la orden.";
    }

    private void Incrementar()
    {
        if (DetalleSeleccionado is null)
        {
            return;
        }

        DetalleSeleccionado.Cantidad++;
        RefrescarTotales();
    }

    private void Decrementar()
    {
        if (DetalleSeleccionado is null)
        {
            return;
        }

        if (DetalleSeleccionado.Cantidad <= 1)
        {
            Detalles.Remove(DetalleSeleccionado);
        }
        else
        {
            DetalleSeleccionado.Cantidad--;
        }

        RefrescarTotales();
    }

    private void RemoverProducto()
    {
        if (DetalleSeleccionado is null)
        {
            return;
        }

        Detalles.Remove(DetalleSeleccionado);
        DetalleSeleccionado = null;
        RefrescarTotales();
    }

    private async Task ProcesarOrdenAsync()
    {
        try
        {
            var request = new CrearOrdenRequest(
                ClienteId,
                EmpleadoId,
                Detalles.Select(d => new CrearOrdenDetalleRequest(d.ProductoId, d.Cantidad)).ToArray());

            var orden = await _apiClient.PostAsync<CrearOrdenRequest, OrdenDto>("api/ordenes", request);
            Mensaje = $"Orden #{orden.NumeroOrden} creada. Total oficial: {orden.Total:C}.";
            Limpiar();
        }
        catch (Exception ex)
        {
            Mensaje = ex.Message;
        }
    }

    private void Limpiar()
    {
        Detalles.Clear();
        DetalleSeleccionado = null;
        RefrescarTotales();
        Mensaje = ClienteId > 0
            ? $"Cliente seleccionado: {ClienteResumen}."
            : "Orden limpia. Seleccione un cliente para continuar.";
        ProcesarOrdenCommand.RaiseCanExecuteChanged();
    }

    private void RefrescarTotales()
    {
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(ImpuestoEstimado));
        OnPropertyChanged(nameof(TotalEstimado));
        ProcesarOrdenCommand.RaiseCanExecuteChanged();
    }
}
