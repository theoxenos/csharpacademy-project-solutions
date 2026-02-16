using Dapper;
using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Services;

namespace FoodJournal.Blazor.Repositories;

public interface IMealRepository
{
    Task<List<Meal>> GetAllMealsAsync();
    Task<Meal?> GetMealByIdAsync(int mealId);
    Task<int> AddMealAsync(Meal meal);
}

public class MealRepository(IDatabaseService databaseService) : IMealRepository
{
    public async Task<List<Meal>> GetAllMealsAsync()
    {
        using var connection = databaseService.GetConnection();
        const string sql = "SELECT * FROM meals";
        return (await connection.QueryAsync<Meal>(sql)).ToList();
    }

    public async Task<Meal?> GetMealByIdAsync(int mealId)
    {
        using var connection = databaseService.GetConnection();
        const string sql = "SELECT * FROM meals WHERE id = @mealId";
        return await connection.QueryFirstOrDefaultAsync<Meal>(sql, new { mealId });
    }

    public async Task<int> AddMealAsync(Meal meal)
    {
        using var connection = databaseService.GetConnection();
        const string sql = """
                           INSERT INTO Meals (Name, Description, ThumbnailUrl, Type, Date) 
                           VALUES (@Name, @Description, @ThumbnailUrl, @Type, @Date);
                           SELECT last_insert_rowid();
                           """;

        var mealId = await connection.ExecuteScalarAsync<int>(sql, meal);

        const string ingredientsSql = """
                                      INSERT INTO MealIngredients (MealId, IngredientName, Measurement)
                                      VALUES (@MealId, @IngredientName, @Measurement);
                                      """;

        await connection.ExecuteAsync(ingredientsSql, meal.Ingredients.Select(i => new
        {
            MealId = mealId,
            IngredientName = i.Ingredient.Name,
            i.Measurement
        }));

        return mealId;
    }
}