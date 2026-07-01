namespace SalesPro.Domain.Entities;

public sealed class Empleado
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string? Email { get; set; }
    public bool Activo { get; set; } = true;
}
