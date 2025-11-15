using ShoppingList.Server.Domain.Entities;
using ShoppingList.Server.Domain.Repositories;

namespace ShoppingList.Server.Application.Services;

public sealed class MealService(IMealRepository mealRepo, IFoodRepository foodRepo) : IMealService
{
    public async Task<Meal> CreateAsync(DateOnly date, MealType type, IEnumerable<(Guid foodId, decimal grams)> items, CancellationToken ct = default)
    {
        var itemList = items?.ToList() ?? [];
        if (itemList.Count == 0)
            throw new ArgumentException("Meal must have at least one item", nameof(items));

        // Validate all foods exist
        foreach (var it in itemList)
        {
            var food = await foodRepo.GetByIdAsync(it.foodId, ct);
            if (food is null)
                throw new InvalidOperationException($"Food {it.foodId} does not exist");
            if (it.grams <= 0)
                throw new ArgumentOutOfRangeException(nameof(items), "Item grams must be > 0");
        }

        var mealItems = itemList.Select(i => new MealItem(Guid.NewGuid(), i.foodId, i.grams)).ToList();
        var meal = new Meal(Guid.NewGuid(), date, type, mealItems);
        return await mealRepo.AddAsync(meal, ct);
    }

    public Task<IReadOnlyList<Meal>> GetByDateAsync(DateOnly date, CancellationToken ct = default)
        => mealRepo.GetByDateAsync(date, ct);

    public Task<Nutrition> GetDailySummaryAsync(DateOnly date, CancellationToken ct = default)
        => mealRepo.GetDailySummaryAsync(date, ct);
}
