using SalesPro.Wpf.Infrastructure;

namespace SalesPro.Wpf.Models;

public sealed class CuentaBancariaFormModel : ViewModelBase
{
    private string _numeroCuenta = string.Empty;
    private string _tipoCuenta = "Corriente";
    private string _tipoDivisa = "CRC";
    private bool _estado = true;
    private string _pais = "Costa Rica";
    private string _provincia = "San José";
    private int _bancoId = 1;
    private int _companiaId = 1;
    private string _nombreDueno = string.Empty;
    private string _apellidosDueno = string.Empty;

    public string NumeroCuenta { get => _numeroCuenta; set => SetProperty(ref _numeroCuenta, value); }
    public string TipoCuenta { get => _tipoCuenta; set => SetProperty(ref _tipoCuenta, value); }
    public string TipoDivisa { get => _tipoDivisa; set => SetProperty(ref _tipoDivisa, value); }
    public bool Estado { get => _estado; set => SetProperty(ref _estado, value); }
    public string Pais { get => _pais; set => SetProperty(ref _pais, value); }
    public string Provincia { get => _provincia; set => SetProperty(ref _provincia, value); }
    public int BancoId { get => _bancoId; set => SetProperty(ref _bancoId, value); }
    public int CompaniaId { get => _companiaId; set => SetProperty(ref _companiaId, value); }
    public string NombreDueno { get => _nombreDueno; set => SetProperty(ref _nombreDueno, value); }
    public string ApellidosDueno { get => _apellidosDueno; set => SetProperty(ref _apellidosDueno, value); }
}
