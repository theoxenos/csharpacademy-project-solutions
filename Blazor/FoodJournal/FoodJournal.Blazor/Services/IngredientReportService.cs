using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Repositories;

namespace FoodJournal.Blazor.Services;

public interface IIngredientReportsService
{
    Task<IReadOnlyList<Ingredient>> GetMostUsedIngredientsAsync(int limit);
}

public class IngredientsReportsService(
    IIngredientsRepository ingredientsRepository,
    IIngredientService ingredientService) : IIngredientReportsService
{
    public async Task<IReadOnlyList<Ingredient>> GetMostUsedIngredientsAsync(int limit)
    {
        var ingredientNames = await ingredientsRepository.GetMostUsedIngredientsName(limit);
        return (await Task.WhenAll(ingredientNames.Select(ingredientService.GetByNameAsync))).OrderBy(i => i.Name)
            .ToList();
    }
}