using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Models.Dto;
using Microsoft.Extensions.Caching.Memory;
using static FoodJournal.Blazor.Utils.IngredientNameNormalizer;

namespace FoodJournal.Blazor.Services;

public interface IIngredientService
{
    Task<List<Ingredient>> GetAllIngredientsAsync();
    Task<Ingredient> GetByNameAsync(string name);
}

public class IngredientService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache) : IIngredientService
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public async Task<List<Ingredient>> GetAllIngredientsAsync()
    {
        var cacheKey = $"{GetType().Name}.Ingredients";

        if (memoryCache.TryGetValue(cacheKey, out List<Ingredient>? cachedIngredients)) return cachedIngredients ?? [];

        await _semaphoreSlim.WaitAsync();
        try
        {
            return await memoryCache.GetOrCreateAsync(cacheKey, async _ => await GetData()) ?? [];
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        async Task<List<Ingredient>> GetData()
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

    public async Task<Ingredient> GetByNameAsync(string name)
    {
        var normalisedName = NormaliseKey(name);
        return (await memoryCache.GetOrCreateAsync
        (
            $"{GetType().Name}.IngredientByName.{normalisedName}", _ => GetByName(normalisedName)
        ))!;

        async Task<Ingredient> GetByName(string name)
        {
            var ingredients = await GetAllIngredientsAsync();
            return ingredients.Single(i => i.Name == name);
        }
    }
}