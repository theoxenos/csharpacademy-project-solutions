using ShoppingList.Server.Domain.Entities;

namespace ShoppingList.Server.Domain.Repositories;

public interface IMealRepository
{
    Task<Meal> AddAsync(Meal meal, CancellationToken ct = default);
    Task<IReadOnlyList<Meal>> GetByDateAsync(DateOnly date, CancellationToken ct = default);

    Task<Nutrition> GetDailySummaryAsync(DateOnly date, CancellationToken ct = default);
}
