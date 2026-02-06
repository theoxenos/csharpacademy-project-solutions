using System.Text.RegularExpressions;
using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Models.Dto;
using Microsoft.Extensions.Caching.Memory;

namespace FoodJournal.Blazor.Services;

public interface IRecipeService
{
    Task<Recipe> GetRecipeByIdAsync(int id);
    Task<List<Recipe>> SearchRecipeByNameAsync(string name);
}

public class RecipeService(
    IHttpClientFactory httpClientFactory,
    IIngredientService ingredientService,
    IMemoryCache memoryCache) : IRecipeService
{
    private const string BaseUrl = "https://www.themealdb.com/api/json/v1/1/";

    public async Task<Recipe> GetRecipeByIdAsync(int id)
    {
        var cacheKey = $"{GetType().Name}.RecipeById.{id}";

        return (await memoryCache.GetOrCreateAsync(cacheKey, async _ => await FetchRecipeData()))!;

        async Task<Recipe?> FetchRecipeData()
        {
            using var httpClient = httpClientFactory.CreateClient();
            var recipeResponse = await httpClient.GetFromJsonAsync<RecipeResponse>($"{BaseUrl}lookup.php?i={id}");
            if (recipeResponse?.Meals is not { Count: > 0 } recipes)
                throw new InvalidOperationException($"Recipe with ID {id} not found");

            return await ParseRecipe(recipes.First());
        }
    }

    public async Task<List<Recipe>> SearchRecipeByNameAsync(string name)
    {
        using var httpClient = httpClientFactory.CreateClient();
        var recipesResponse =
            await httpClient.GetFromJsonAsync<RecipeResponse>(
                $"{BaseUrl}search.php?s={name}");

        if (recipesResponse?.Meals == null) return [];

        var recipeTasks = recipesResponse.Meals.Select(ParseRecipe);

        var recipes = (await Task.WhenAll(recipeTasks)).ToList();
        return recipes;
    }

    private int ParseKeyIndex(string key)
    {
        var match = new Regex(@"\d+").Match(key);
        return int.Parse(match.Value);
    }

    private async Task<Recipe> ParseRecipe(Dictionary<string, string> meal)
    {
        var ingredientTasks = meal
            .Where(mealProperty =>
                mealProperty.Key.Contains("strIngredient") && !string.IsNullOrEmpty(mealProperty.Value))
            .Select(async mealProperty =>
            {
                var ingredient = await ingredientService.GetByNameAsync(mealProperty.Value);
                return new RecipeIngredient
                {
                    Ingredient = ingredient,
                    Measurement = meal[$"strMeasure{ParseKeyIndex(mealProperty.Key)}"]
                };
            });

        var ingredients = (await Task.WhenAll(ingredientTasks)).ToList();

        return new Recipe
        {
            Id = int.Parse(meal["idMeal"]),
            Name = meal["strMeal"],
            Description = meal["strInstructions"],
            ThumbnailUrl = meal["strMealThumb"],
            Ingredients = ingredients
        };
    }
}