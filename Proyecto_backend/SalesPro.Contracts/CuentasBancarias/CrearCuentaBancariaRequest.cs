namespace SalesPro.Contracts.CuentasBancarias;

public sealed record CrearCuentaBancariaRequest(
    string NumeroCuenta,
    string TipoCuenta,
    string TipoDivisa,
    bool Estado,
    string Pais,
    string Provincia,
    int BancoId,
    int CompaniaId,
    string NombreDueno,
    string ApellidosDueno);
