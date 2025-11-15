using ShoppingList.Server.Models;

namespace ShoppingList.Server.Repositories;

public interface IShoppingListRepository : IRepository<ShoppingListModel>
{
    Task<ShoppingListModel> AddAsync(ShoppingListModel entity);
    Task<List<ShoppingListModel>> GetAllAsync();
    Task<ShoppingListModel?> GetOneAsync(int id);
}