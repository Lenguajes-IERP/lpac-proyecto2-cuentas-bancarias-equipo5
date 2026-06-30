namespace SalesPro.Domain.Entities;

public sealed class Banco
{
    public int Id { get; set; }
    public string CodigoIdentificadorBanco { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Ciudad { get; set; }
    public string? Pais { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public bool Active { get; set; } = true;
}
