using Microsoft.Data.Sqlite;
using ShoppingList.Server.Models;
using ShoppingList.Server.Repositories;

namespace ShoppingList.Server.Services;

public class ItemService(IItemRepository repository) : IItemService
{
    public async Task<ItemModel> CreateItemAsync(int shoppingListId, string name, int quantity)
    {
        var newItem = new ItemModel
        {
            ShoppingListId = shoppingListId, Name = name, Quantity = quantity, CreatedAt = DateTime.UtcNow
        };

        try
        {
            return await repository.AddAsync(newItem);
        }
        catch (SqliteException exception) when (exception.SqliteExtendedErrorCode == 787)
        {
            // SQLite error code 787 is SQLITE_CONSTRAINT_FOREIGNKEY
            throw new InvalidOperationException(
                $"Shopping list with ID {shoppingListId} does not exist.",
                exception);
        }
    }

    public Task<List<ItemModel>> GetAllAsync() => repository.GetAllAsync();

    public Task<ItemModel?> GetOneAsync(int id)
    {
        return repository.GetOneAsync(id);
    }

    public Task<List<ItemModel>> GetByShoppingListIdAsync(int id)
    {
        return repository.GetByShoppingListIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var result = await repository.DeleteAsync(id);
        if (result == 0) throw new InvalidOperationException($"Item with ID {id} does not exist.");
    }

    public async Task<ItemModel> UpdateAsync(int id, int shoppingListId, string name, int quantity, bool isChecked)
    {
        var item = new ItemModel
        {
            Id = id,
            ShoppingListId = shoppingListId,
            Name = name,
            Quantity = quantity,
            IsChecked = isChecked,
            ModifiedAt = DateTime.UtcNow

        };
        var updatedItem = await repository.UpdateAsync(item);

        return updatedItem ?? throw new InvalidOperationException($"Item with ID {item.Id} does not exist.");
    }
}