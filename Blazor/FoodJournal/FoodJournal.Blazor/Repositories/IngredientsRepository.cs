using Dapper;
using FoodJournal.Blazor.Services;

namespace FoodJournal.Blazor.Repositories;

public interface IIngredientsRepository
{
    Task<List<(string Ingredient, string Measurement)>> GetAllIngredientsByMealIdAsync(int mealId);
    Task<List<string>> GetMostUsedIngredientsName(int limit = -1);
}

public class IngredientsRepository(IDatabaseService databaseService) : IIngredientsRepository
{
    public async Task<List<(string Ingredient, string Measurement)>> GetAllIngredientsByMealIdAsync(int mealId)
    {
        const string sql = "SELECT * FROM MealIngredients WHERE MealId = @mealId";
        using var connection = databaseService.GetConnection();
        var ingredients = await connection.QueryAsync(sql, new { mealId });
        return ingredients.Select(i =>
            (
                Ingredient: (string)i.IngredientName,
                Measure: (string)i.Measurement
            )
        ).ToList();
    }

    public async Task<List<string>> GetMostUsedIngredientsName(int limit = -1)
    {
        const string sql = """
                           SELECT MealIngredients.IngredientName
                           FROM MealIngredients
                           GROUP BY MealIngredients.IngredientName
                           ORDER BY COUNT(*) DESC
                           LIMIT @Limit
                           """;

        using var connection = databaseService.GetConnection();

        var names = await connection.QueryAsync<string>(sql, new { Limit = limit });
        return names.AsList();
    }
}