using ShoppingList.Server.Domain.Entities;

namespace ShoppingList.Server.Application.Services;

public interface IFoodService
{
    Task<Food> CreateAsync(string name, Nutrition per100g, CancellationToken ct = default);
    Task<IReadOnlyList<Food>> ListAsync(CancellationToken ct = default);
}
