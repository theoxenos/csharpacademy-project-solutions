using ShoppingList.Server.Dtos;
using ShoppingList.Server.Models;

namespace ShoppingList.Server.Services;

public interface IItemService
{
    Task<ItemModel> CreateItemAsync(int shoppingListId, string name, int quantity);
    Task<List<ItemModel>> GetByShoppingListIdAsync(int id);
    Task<List<ItemModel>> GetAllAsync();
    Task<ItemModel?> GetOneAsync(int id);
    Task DeleteAsync(int id);
    Task<ItemModel> UpdateAsync(int id, int shoppingListId, string name, int quantity, bool isChecked);
}