using Dapper;
using ShoppingList.Server.Database;
using ShoppingList.Server.Models;

namespace ShoppingList.Server.Repositories;

public class ItemRepository(IDatabaseConnectionFactory databaseConnectionFactory) : IItemRepository
{
    public async Task<ItemModel> AddAsync(ItemModel entity)
    {
        const string sql = """
                           INSERT INTO Items (ShoppingListId, Name, Quantity, IsChecked, CreatedAt, ModifiedAt) 
                           VALUES (@ShoppingListId, @Name, @Quantity, @IsChecked, @CreatedAt, @CreatedAt);
                           SELECT * FROM Items WHERE Id = last_insert_rowid();
                           """;
        using var connection = databaseConnectionFactory.CreateConnection();
        return await connection.QuerySingleAsync<ItemModel>(sql, entity);
    }

    public async Task<List<ItemModel>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Items";
        using var connection = databaseConnectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql);

        return rows.Select(r => new ItemModel
        {
            Id = (int)r.Id,
            ShoppingListId = (int)r.ShoppingListId,
            Name = (string)r.Name,
            Quantity = (int)r.Quantity,
            IsChecked = (bool)r.IsChecked,
            CreatedAt = DateTime.Parse((string)r.CreatedAt)
        }).ToList();
    }

    public Task<ItemModel?> GetOneAsync(int id)
    {
        const string sql = "SELECT * FROM Items WHERE Id = @Id";
        var connection = databaseConnectionFactory.CreateConnection();
        return connection.QuerySingleOrDefaultAsync<ItemModel>(sql, new { Id = id });
    }

    public async Task<List<ItemModel>> GetByShoppingListIdAsync(int id)
    {
        const string sql = "SELECT * FROM Items WHERE ShoppingListId = @Id";
        using var connection = databaseConnectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<ItemModel>(sql, new { Id = id });
        return rows.ToList();
    }
}