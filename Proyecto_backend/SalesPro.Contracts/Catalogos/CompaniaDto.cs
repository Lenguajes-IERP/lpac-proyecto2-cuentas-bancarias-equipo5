namespace SalesPro.Contracts.Catalogos;

public sealed record CompaniaDto(
    int Id,
    string Nombre,
    string? CedulaJuridica);
