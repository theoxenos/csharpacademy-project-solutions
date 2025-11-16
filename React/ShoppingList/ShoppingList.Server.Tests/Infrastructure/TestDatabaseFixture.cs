using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using ShoppingList.Server.Database;

namespace ShoppingList.Server.Tests.Infrastructure;

// Keeps a shared in-memory SQLite database alive for the duration of the test run
public class TestDatabaseFixture : IDisposable
{
    private readonly string _connectionString = "Data Source=ShoppingListTests;Mode=Memory;Cache=Shared";
    private readonly SqliteConnection _keeperConnection;
    public IDatabaseConnectionFactory ConnectionFactory { get; }

    public TestDatabaseFixture()
    {
        _keeperConnection = new SqliteConnection(_connectionString);
        _keeperConnection.Open();

        // Ensure foreign keys are enabled for the session
        _keeperConnection.Execute("PRAGMA foreign_keys = ON;");

        ConnectionFactory = new InMemoryConnectionFactory(_connectionString);

        // Create schema once
        var initialiser = new DatabaseInitialiser(ConnectionFactory);
        initialiser.InitialiseAsync().GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        _keeperConnection.Dispose();
        (ConnectionFactory as IDisposable)?.Dispose();
    }

    private sealed class InMemoryConnectionFactory(string connectionString) : IDatabaseConnectionFactory, IDisposable
    {
        public IDbConnection CreateConnection()
        {
            var conn = new SqliteConnection(connectionString);
            conn.Open();
            // Ensure foreign keys are enabled on each new connection
            conn.Execute("PRAGMA foreign_keys = ON;");
            return conn;
        }

        public void Dispose()
        {
        }
    }
}
