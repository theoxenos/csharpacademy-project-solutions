using ShoppingList.Server.Models;

namespace ShoppingList.Server.Services;

public interface IShoppingListService
{
    Task<ShoppingListModel> CreateShoppingListAsync(string name);
    Task<List<ShoppingListModel>> GetAllShoppingListsAsync();
    Task<ShoppingListModel?> GetShoppingListAsync(int id);
}