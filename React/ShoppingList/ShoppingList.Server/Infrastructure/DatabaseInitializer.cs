using System.Data;
using Dapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShoppingList.Server.Abstractions;

namespace ShoppingList.Server.Infrastructure;

public sealed class DatabaseInitializer(
    IDbConnectionFactory connectionFactory,
    ILogger<DatabaseInitializer> logger,
    IHostEnvironment env)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Ensure App_Data exists for SQLite file path
            var dataDir = Path.Combine(env.ContentRootPath, "App_Data");
            Directory.CreateDirectory(dataDir);

            using var connection = connectionFactory.CreateConnection();
            if (connection.State != ConnectionState.Open)
                await ((dynamic)connection).OpenAsync(cancellationToken);

            // Ensure foreign keys
            await connection.ExecuteAsync("PRAGMA foreign_keys = ON;");

            var sql = @"
CREATE TABLE IF NOT EXISTS Foods (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    CaloriesPer100g REAL NOT NULL,
    ProteinPer100g REAL NOT NULL,
    CarbsPer100g REAL NOT NULL,
    FatPer100g REAL NOT NULL,
    CreatedAt TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Meals (
    Id TEXT PRIMARY KEY,
    Date TEXT NOT NULL,
    Type INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS MealItems (
    Id TEXT PRIMARY KEY,
    MealId TEXT NOT NULL,
    FoodId TEXT NOT NULL,
    QuantityGrams REAL NOT NULL,
    FOREIGN KEY (MealId) REFERENCES Meals(Id) ON DELETE CASCADE,
    FOREIGN KEY (FoodId) REFERENCES Foods(Id)
);
";
            await connection.ExecuteAsync(sql);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during database initialization");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}