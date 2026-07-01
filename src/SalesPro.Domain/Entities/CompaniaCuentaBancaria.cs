namespace SalesPro.Domain.Entities;

public sealed class CompaniaCuentaBancaria
{
    public int Id { get; set; }
    public string NumeroCuenta { get; set; } = string.Empty;
    public string TipoCuenta { get; set; } = string.Empty;
    public string TipoDivisa { get; set; } = string.Empty;
    public bool Estado { get; set; } = true;
    public string Pais { get; set; } = string.Empty;
    public string Provincia { get; set; } = string.Empty;
    public int BancoId { get; set; }
    public int CompaniaId { get; set; }
    public string NombreDueno { get; set; } = string.Empty;
    public string ApellidosDueno { get; set; } = string.Empty;
}
