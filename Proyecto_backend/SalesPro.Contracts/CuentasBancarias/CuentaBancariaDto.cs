namespace SalesPro.Contracts.CuentasBancarias;

public sealed record CuentaBancariaDto(
    int Id,
    string NumeroCuenta,
    string TipoCuenta,
    string TipoDivisa,
    bool Estado,
    string Pais,
    string Provincia,
    int BancoId,
    string BancoNombre,
    int CompaniaId,
    string CompaniaNombre,
    string NombreDueno,
    string ApellidosDueno);
