using ShoppingList.Server.Models;
using ShoppingList.Server.Repositories;

namespace ShoppingList.Server.Services;

public class ShoppingListService(IShoppingListRepository repository) : IShoppingListService
{
    public async Task<ShoppingListModel> CreateShoppingListAsync(string name)
    {
        var now = DateTime.UtcNow;
        var shoppingList = new ShoppingListModel {Name = name, CreatedAt = now, ModifiedAt = now};
        
        return await repository.AddAsync(shoppingList);
    }
    
    public async Task<List<ShoppingListModel>> GetAllShoppingListsAsync()
    {
        return await repository.GetAllAsync();
    }
    
    public async Task<ShoppingListModel?> GetShoppingListAsync(int id)
    {
        return await repository.GetOneAsync(id);
    }

    public async Task<ShoppingListModel?> UpdateShoppingListAsync(int id, string name)
    {
        var now = DateTime.UtcNow;
        return await repository.UpdateAsync(new ShoppingListModel { Id = id, Name = name, ModifiedAt = now});
    }
}