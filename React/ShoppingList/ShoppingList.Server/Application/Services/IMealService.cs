using ShoppingList.Server.Domain.Entities;

namespace ShoppingList.Server.Application.Services;

public interface IMealService
{
    Task<Meal> CreateAsync(DateOnly date, MealType type, IEnumerable<(Guid foodId, decimal grams)> items, CancellationToken ct = default);
    Task<IReadOnlyList<Meal>> GetByDateAsync(DateOnly date, CancellationToken ct = default);
    Task<Nutrition> GetDailySummaryAsync(DateOnly date, CancellationToken ct = default);
}
