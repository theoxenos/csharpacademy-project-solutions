using Dapper;
using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Services;

namespace FoodJournal.Blazor.Repositories;

public interface IMealRepository
{
    Task<List<Meal>> GetAllMealsAsync();
    Task<Meal?> GetMealByIdAsync(int mealId);
    Task<int> AddMealAsync(Meal meal);
    Task UpdateMealAsync(Meal meal);
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

    public async Task UpdateMealAsync(Meal meal)
    {
        using var connection = databaseService.GetConnection();
        const string sql = """
                           UPDATE Meals SET Name = @Name, Description = @Description, ThumbnailUrl = @ThumbnailUrl, Type = @Type, Date = @Date WHERE Id = @Id;
                           """;

        await connection.ExecuteAsync(sql, meal);

        const string deleteIngredientsSql = "DELETE FROM MealIngredients WHERE MealId = @MealId";
        await connection.ExecuteAsync(deleteIngredientsSql, new { MealId = meal.Id });

        const string insertIngredientsSql = """
                                            INSERT INTO MealIngredients (MealId, IngredientName, Measurement)
                                            VALUES (@MealId, @IngredientName, @Measurement);
                                            """;

        await connection.ExecuteAsync(insertIngredientsSql, meal.Ingredients.Select(i => new
        {
            MealId = meal.Id,
            IngredientName = i.Ingredient.Name,
            i.Measurement
        }));
    }
}