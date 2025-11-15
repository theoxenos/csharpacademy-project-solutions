using Dapper;

namespace ShoppingList.Server.Database;

public class DatabaseInitialiser(IDatabaseConnectionFactory databaseConnectionFactory)
{
    public async Task InitialiseAsync()
    {
        var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
        Directory.CreateDirectory(dataDir);
        
        using var connection = databaseConnectionFactory.CreateConnection();

        const string sql = """
                           CREATE TABLE IF NOT EXISTS ShoppingLists (
                               Id INTEGER PRIMARY KEY AUTOINCREMENT,
                               Name TEXT NOT NULL,
                               CreatedAt TEXT NOT NULL,
                               ModifiedAt TEXT NOT NULL
                           );

                           CREATE TABLE IF NOT EXISTS Items (
                               Id INTEGER PRIMARY KEY AUTOINCREMENT,
                               ShoppingListId INTEGER NOT NULL,
                               Name TEXT NOT NULL,
                               Quantity INTEGER NOT NULL,
                               IsChecked BOOLEAN NOT NULL,
                               CreatedAt TEXT NOT NULL,
                               ModifiedAt TEXT NOT NULL,
                               FOREIGN KEY (ShoppingListId) REFERENCES ShoppingLists(Id) ON DELETE CASCADE
                           )
                           """;

        await connection.ExecuteAsync(sql);
    }
}