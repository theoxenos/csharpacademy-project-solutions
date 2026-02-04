using System.Text.RegularExpressions;
using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Models.Dto;
using FoodJournal.Blazor.Repositories;

namespace FoodJournal.Blazor.Services;

public class RecipeService(HttpClient httpClient, IngredientsRepository ingredientsRepository)
{
    public async Task<List<Recipe>> SearchRecipeByNameAsync(string name)
    {
        var recipesResponse =
            await httpClient.GetFromJsonAsync<RecipeResponse>(
                $"https://www.themealdb.com/api/json/v1/1/search.php?s={name}");

        if (recipesResponse?.Meals == null) return [];

        var recipeTasks = recipesResponse.Meals.Select(async r =>
        {
            var ingredientTasks = r
                .Where(x => x.Key.Contains("strIngredient") && !string.IsNullOrEmpty(x.Value))
                .Select(async x =>
                {
                    var ingredient = await ingredientsRepository.GetByNameAsync(x.Value);
                    return new RecipeIngredient
                    {
                        Ingredient = ingredient,
                        Measurement = r[$"strMeasure{ParseKeyIndex(x.Key)}"]
                    };
                });

            var ingredients = (await Task.WhenAll(ingredientTasks)).ToList();

            return new Recipe
            {
                Id = Random.Shared.Next(1, 1_000_000),
                Name = r["strMeal"],
                Description = r["strInstructions"],
                ThumbnailUrl = r["strMealThumb"],
                Ingredients = ingredients
            };
        });

        var recipes = (await Task.WhenAll(recipeTasks)).ToList();
        return recipes;
    }

    private int ParseKeyIndex(string key)
    {
        var match = new Regex(@"\d+").Match(key);
        return int.Parse(match.Value);
    }
}