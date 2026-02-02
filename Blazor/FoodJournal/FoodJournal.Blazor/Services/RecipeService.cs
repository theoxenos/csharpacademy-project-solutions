using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Models.Dto;

namespace FoodJournal.Blazor.Services;

public class RecipeService(HttpClient httpClient)
{
    public async Task<List<Recipe>> SearchRecipeByNameAsync(string name)
    {
        var recipesResponse =
            await httpClient.GetFromJsonAsync<RecipeResponse>(
                $"https://www.themealdb.com/api/json/v1/1/search.php?s={name}");

        if (recipesResponse?.Meals == null)
        {
            return [];
        }

        return recipesResponse.Meals.Select(r =>
        {
            var ingredients = r
                .Where(x => x.Key.Contains("strIngredient") && !string.IsNullOrEmpty(x.Value))
                .Select(x => new Ingredient { Name = x.Value })
                .ToList();

            return new Recipe
            {
                Name = r["strMeal"],
                Description = r["strInstructions"],
                Ingredients = ingredients,
                ThumbnailUrl = r["strMealThumb"]
            };
        }).ToList();
    }
}