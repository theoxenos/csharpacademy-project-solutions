using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using ShoppingList.Server.Abstractions;

namespace ShoppingList.Server.Infrastructure;

public sealed class SqliteConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? "Data Source=:memory:";
        var connection = new SqliteConnection(connectionString);
        return connection;
    }
}
