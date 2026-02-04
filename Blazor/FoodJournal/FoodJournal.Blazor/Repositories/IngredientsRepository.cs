using Dapper;
using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Services;
using static FoodJournal.Blazor.Utils.IngredientNameNormalizer;

namespace FoodJournal.Blazor.Repositories;

public class IngredientsRepository(DatabaseService databaseService)
{
    public async Task<Ingredient> AddAsync(Ingredient entity)
    {
        const string sql =
            """
            INSERT INTO Ingredients (Name, Description, ThumbnailUrl, Type, Calories, Carbohydrates, Fat, Protein) 
            VALUES (@Name, @Description, @ThumbnailUrl, @Type, @Calories, @Carbohydrates, @Fat, @Protein) 
            RETURNING *
            """;
        using var connection = databaseService.GetConnection();
        var result = await connection.QueryAsync<Ingredient>(sql, entity);
        return result.First();
    }

    public async Task<IEnumerable<Ingredient>> AddManyAsync(IEnumerable<Ingredient> entities)
    {
        const string sql =
            """
            INSERT INTO Ingredients (Name, Description, ThumbnailUrl, Type, Calories, Carbohydrates, Fat, Protein) 
            VALUES (@Name, @Description, @ThumbnailUrl, @Type, @Calories, @Carbohydrates, @Fat, @Protein) 
            RETURNING *
            """;
        using var connection = databaseService.GetConnection();
        var result = await connection.QueryAsync<Ingredient>(sql, entities);
        return result.ToList();
    }

    public async Task<Ingredient> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM Ingredients WHERE Id = @Id";
        using var connection = databaseService.GetConnection();
        var ingredient = await connection.QueryFirstOrDefaultAsync<Ingredient>(sql, new { Id = id });
        return ingredient ?? throw new InvalidOperationException($"Ingredient with id '{id}' not found");
    }

    public async Task<Ingredient> GetByNameAsync(string name)
    {
        name = NormaliseKey(name);
        const string sql = "SELECT * FROM Ingredients WHERE Name = @Name";
        using var connection = databaseService.GetConnection();
        var ingredient = await connection.QueryFirstOrDefaultAsync<Ingredient>(sql, new { Name = name });
        return ingredient ?? throw new InvalidOperationException($"Ingredient with name '{name}' not found");
    }

    public async Task<List<Ingredient>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Ingredients";
        using var connection = databaseService.GetConnection();
        return (await connection.QueryAsync<Ingredient>(sql)).ToList();
    }

    public async Task<Ingredient> UpdateAsync(Ingredient entity)
    {
        const string sql =
            """
            UPDATE Ingredients SET Name = @Name, Description = @Description, ThumbnailUrl = @ThumbnailUrl WHERE Id = @Id
            """;
        using var connection = databaseService.GetConnection();
        await connection.ExecuteAsync(sql, entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Ingredients WHERE Id = @Id";
        using var connection = databaseService.GetConnection();
        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
        if (affectedRows > 1)
            throw new InvalidOperationException($"More than one ingredient with id '{id}' was deleted");
        return affectedRows == 1;
    }
}