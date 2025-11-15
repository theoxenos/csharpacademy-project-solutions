using Dapper;
using ShoppingList.Server.Database;
using ShoppingList.Server.Models;

namespace ShoppingList.Server.Repositories;

public class ShoppingListRepository(IDatabaseConnectionFactory databaseConnectionFactory) : IShoppingListRepository
{
    public async Task<ShoppingListModel> AddAsync(ShoppingListModel entity)
    {
        const string sql = """
                           INSERT INTO ShoppingLists (Name, CreatedAt, ModifiedAt) VALUES (@Name, @CreatedAt, @CreatedAt); 
                           SELECT * FROM ShoppingLists WHERE Id = last_insert_rowid();
                           """;
        using var connection = databaseConnectionFactory.CreateConnection();
        return await connection.QuerySingleAsync<ShoppingListModel>(sql, entity);
    }

    public async Task<List<ShoppingListModel>> GetAllAsync()
    {
        const string sql = "SELECT * FROM ShoppingLists";
        using var connection = databaseConnectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<ShoppingListModel>(sql);
        return rows.ToList();
    }

    public async Task<ShoppingListModel?> GetOneAsync(int id)
    {
        const string sql = "SELECT * FROM ShoppingLists WHERE Id = @Id";
        using var connection = databaseConnectionFactory.CreateConnection();
        var row = await connection.QuerySingleOrDefaultAsync<ShoppingListModel>(sql, new { Id = id });
        return row ?? null;
    }
}