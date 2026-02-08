using Dapper;
using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Services;

namespace FoodJournal.Blazor.Repositories;

public interface IMealRepository
{
    Task<List<Meal>> GetAllMealsAsync();
    Task<Meal?> GetMealByIdAsync(int mealId);
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
}