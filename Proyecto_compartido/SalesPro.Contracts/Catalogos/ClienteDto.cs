namespace SalesPro.Contracts.Catalogos;

public sealed record ClienteDto(
    int Id,
    string Nombre,
    string Apellidos,
    string NumeroIdentificacion,
    string? Email);
