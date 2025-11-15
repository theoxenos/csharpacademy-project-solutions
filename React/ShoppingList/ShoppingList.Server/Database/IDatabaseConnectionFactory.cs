using System.Data;

namespace ShoppingList.Server.Database;

public interface IDatabaseConnectionFactory
{
    public IDbConnection CreateConnection();
}