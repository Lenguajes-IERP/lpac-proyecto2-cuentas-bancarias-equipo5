namespace SalesPro.Data.Repositories;

public interface IParametroSistemaRepository
{
    Task<decimal?> ObtenerValorDecimalAsync(string nombre, CancellationToken cancellationToken);
}
