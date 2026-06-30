using Microsoft.Data.SqlClient;

namespace SalesPro.Data.Infrastructure;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}
