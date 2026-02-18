using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Repositories;

namespace FoodJournal.Blazor.Services;

public interface IMealService
{
    Task<Meal> AddMealAsync(Meal meal);
    Task<List<Meal>> GetAllMealsAsync();
    Task<Meal?> GetMealByIdAsync(int mealId);
    Task<Meal> UpdateMealAsync(Meal meal);
}

public class MealService(
    IIngredientService ingredientService,
    IIngredientsRepository ingredientsRepository,
    IMealRepository mealRepository) : IMealService
{
    public async Task<Meal> AddMealAsync(Meal meal)
    {
        var mealId = await mealRepository.AddMealAsync(meal);
        var mealIngredients = await GetAllRecipeIngredientsByMealIdAsync(mealId);

        return new Meal
        {
            Id = mealId,
            Name = meal.Name,
            Date = meal.Date,
            Description = meal.Description,
            Ingredients = mealIngredients,
            ThumbnailUrl = meal.ThumbnailUrl,
            Type = meal.Type
        };
    }

    public async Task<List<Meal>> GetAllMealsAsync()
    {
        var databaseMeals = await mealRepository.GetAllMealsAsync();

        if (databaseMeals is not { Count: > 0 }) return [];

        var mealTasks = databaseMeals.Select(async meal =>
        {
            var mealIngredients = await GetAllRecipeIngredientsByMealIdAsync(meal.Id);

            meal.Ingredients = mealIngredients;

            return meal;
        });

        return (await Task.WhenAll(mealTasks)).ToList();
    }

    public async Task<Meal?> GetMealByIdAsync(int mealId)
    {
        var meal = await mealRepository.GetMealByIdAsync(mealId);
        if (meal == null) return null;
        var mealIngredients = await GetAllRecipeIngredientsByMealIdAsync(meal.Id);
        meal.Ingredients = mealIngredients;
        return meal;
    }

    public async Task<Meal> UpdateMealAsync(Meal meal)
    {
        await mealRepository.UpdateMealAsync(meal);
        meal.Ingredients = await GetAllRecipeIngredientsByMealIdAsync(meal.Id);
        return meal;
    }

    private async Task<List<RecipeIngredient>> GetAllRecipeIngredientsByMealIdAsync(int mealId)
    {
        var ingredients = await ingredientsRepository.GetAllIngredientsByMealIdAsync(mealId);
        var mealIngredientsTasks = ingredients.Select(async i =>
        {
            var ingredient = await ingredientService.GetByNameAsync(i.Ingredient);
            return new RecipeIngredient
            {
                Ingredient = ingredient,
                Measurement = i.Measurement
            };
        });

        var mealIngredients = await Task.WhenAll(mealIngredientsTasks);
        return mealIngredients.ToList();
    }
}