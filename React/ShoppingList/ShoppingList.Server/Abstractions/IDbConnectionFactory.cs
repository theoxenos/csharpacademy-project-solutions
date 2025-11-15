using System.Data;

namespace ShoppingList.Server.Abstractions;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
