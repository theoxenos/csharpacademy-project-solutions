using ShoppingList.Server.Domain.Entities;

namespace ShoppingList.Server.Domain.Repositories;

public interface IFoodRepository
{
    Task<Food> AddAsync(Food food, CancellationToken ct = default);
    Task<IReadOnlyList<Food>> GetAllAsync(CancellationToken ct = default);
    Task<Food?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
