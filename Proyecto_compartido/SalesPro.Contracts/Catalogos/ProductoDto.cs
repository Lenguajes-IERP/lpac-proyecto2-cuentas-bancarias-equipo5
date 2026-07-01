namespace SalesPro.Contracts.Catalogos;

public sealed record ProductoDto(
    int ProductoId,
    string? CodigoBarra,
    string NombreEtiqueta,
    string? Description,
    int ExistenciaEnStock,
    decimal PrecioNeto,
    bool PuedeVenderse,
    bool TieneImpuesto);
