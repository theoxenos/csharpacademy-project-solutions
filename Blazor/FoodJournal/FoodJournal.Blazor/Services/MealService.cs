using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Repositories;

namespace FoodJournal.Blazor.Services;

public interface IMealService
{
    Task<Meal> AddMealAsync(Meal meal);
}

public class MealService(
    IIngredientService ingredientService,
    IIngredientsRepository ingredientsRepository,
    IMealRepository mealRepository) : IMealService
{
    public async Task<Meal> AddMealAsync(Meal meal)
    {
        var mealId = await mealRepository.AddMealAsync(meal);
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

        return new Meal
        {
            Id = mealId,
            Name = meal.Name,
            Description = meal.Description,
            Ingredients = mealIngredients.ToList()
        };
    }
}