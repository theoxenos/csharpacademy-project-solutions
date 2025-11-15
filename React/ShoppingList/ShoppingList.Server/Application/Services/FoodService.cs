using ShoppingList.Server.Domain.Entities;
using ShoppingList.Server.Domain.Repositories;

namespace ShoppingList.Server.Application.Services;

public sealed class FoodService(IFoodRepository repo) : IFoodService
{
    public async Task<Food> CreateAsync(string name, Nutrition per100g, CancellationToken ct = default)
    {
        var food = new Food(Guid.NewGuid(), name, per100g);
        return await repo.AddAsync(food, ct);
    }

    public Task<IReadOnlyList<Food>> ListAsync(CancellationToken ct = default)
        => repo.GetAllAsync(ct);
}
