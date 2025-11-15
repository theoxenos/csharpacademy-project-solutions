using System.Data;
using Microsoft.Data.Sqlite;

namespace ShoppingList.Server.Database;

public class SqliteConnectionFactory(IConfiguration configuration) : IDatabaseConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=:memory:";
        var connection = new SqliteConnection(connectionString);
        return connection;
    }
}