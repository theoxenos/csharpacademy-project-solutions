using System.Data;
using System.Reflection;
using Dapper;
using Microsoft.Data.Sqlite;

namespace FoodJournal.Blazor.Services;

public class DatabaseService(IConfiguration configuration, IngredientService ingredientService)
{
    public IDbConnection GetConnection()
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        return new SqliteConnection(connectionString);
    }

    public async Task CreateDatabase()
    {
        await CreateTables();
        await SeedDatabase();
    }

    private async Task SeedDatabase()
    {
        await SeedIngredients();
    }

    private async Task SeedIngredients()
    {
        using var connection = GetConnection();

        const string seedHistorySql = "SELECT COUNT(*) FROM SeedHistory WHERE TableName = 'Ingredients'";
        var seedHistory = await connection.ExecuteScalarAsync<int>(seedHistorySql);
        if (seedHistory > 0) return;

        var ingredients = await ingredientService.GetAllIngredientsAsync();
        const string sql =
            """
            INSERT INTO Ingredients 
                (Id, Name, Description, ThumbnailUrl, Type, Calories, Carbohydrates, Protein, Fat) 
            VALUES 
                (null, @Name, @Description, @ThumbnailUrl, @Type, @Calories, @Carbohydrates, @Protein, @Fat);
            """;
        await connection.ExecuteAsync(sql, ingredients);
        await connection.ExecuteAsync("INSERT INTO SeedHistory (TableName) VALUES ('Ingredients')");
    }

    private async Task CreateTables()
    {
        using var connection = GetConnection();

        var seedHistorySql = await GetSqlFromResource("FoodJournal.Blazor.Scripts.000_CreateSeedHistory.sql");
        await connection.ExecuteAsync(seedHistorySql);

        var ingredientsSql = await GetSqlFromResource("FoodJournal.Blazor.Scripts.001_CreateIngredients.sql");
        await connection.ExecuteAsync(ingredientsSql);
    }

    private async Task<string> GetSqlFromResource(string name)
    {
        await using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        if (stream == null) throw new InvalidOperationException($"Resource '{name}' not found");

        using var reader = new StreamReader(stream);

        return await reader.ReadToEndAsync();
    }
}