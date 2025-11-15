using ShoppingList.Server.Models;

namespace ShoppingList.Server.Repositories;

public interface IItemRepository : IRepository<ItemModel>
{
    public Task<List<ItemModel>> GetByShoppingListIdAsync(int id);
}