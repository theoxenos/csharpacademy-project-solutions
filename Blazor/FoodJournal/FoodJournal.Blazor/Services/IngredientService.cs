using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Models.Dto;
using static FoodJournal.Blazor.Utils.IngredientNameNormalizer;

namespace FoodJournal.Blazor.Services;

public class IngredientService(IHttpClientFactory httpClientFactory)
{
    public async Task<List<Ingredient>> GetAllIngredientsAsync()
    {
        using var httpClient = httpClientFactory.CreateClient();
        var rng = new Random();
        var ingredients =
            await httpClient.GetFromJsonAsync<RecipeResponse>(
                "https://www.themealdb.com/api/json/v1/1/list.php?i=list");
        return ingredients?.Meals.Select(m => new Ingredient
        {
            Id = int.Parse(m["idIngredient"]),
            Name = NormaliseKey(m["strIngredient"]),
            Description = m["strDescription"],
            ThumbnailUrl = m["strThumb"],
            Type = m["strType"],
            Calories = rng.Next(500, 5_000),
            Carbohydrates = rng.Next(10, 100),
            Protein = rng.Next(5, 50),
            Fat = rng.Next(5, 50)
        }).ToList() ?? [];
    }
}