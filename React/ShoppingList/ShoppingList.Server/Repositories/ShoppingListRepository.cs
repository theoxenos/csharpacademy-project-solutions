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
        const string sql = """
                           SELECT * FROM ShoppingLists
                           LEFT JOIN Items ON ShoppingLists.Id = Items.ShoppingListId;
                           """;
        using var connection = databaseConnectionFactory.CreateConnection();
        var shoppingListsDictionary = new Dictionary<int, ShoppingListModel>();

        var rows = await connection.QueryAsync<ShoppingListModel, ItemModel, ShoppingListModel>(
            sql,
            (list, listItem) =>
            {
                if (!shoppingListsDictionary.TryGetValue(list.Id, out var listEntry))
                {
                    listEntry = list;
                    shoppingListsDictionary.Add(list.Id, listEntry);
                }
                
                listEntry.Items.Add(listItem);
                return listEntry;
            },
            splitOn: "Id");
        return rows.ToList();
    }

    public async Task<ShoppingListModel?> GetOneAsync(int id)
    {
        const string sql = """
                           SELECT * 
                           FROM ShoppingLists sl
                           LEFT JOIN Items 
                           ON sl.Id = Items.ShoppingListId
                           WHERE sl.Id = @Id;
                           """;
        using var connection = databaseConnectionFactory.CreateConnection();
        var shoppingListsDictionary = new Dictionary<int, ShoppingListModel>();

        var row = await connection.QueryAsync<ShoppingListModel, ItemModel, ShoppingListModel>(
            sql,
            (list, listItem) =>
            {
                if (!shoppingListsDictionary.TryGetValue(list.Id, out var listEntry))
                {
                    listEntry = list;
                    shoppingListsDictionary.Add(list.Id, listEntry);
                }

                listEntry.Items.Add(listItem);
                return listEntry;
            },
            new { Id = id },
            splitOn: "Id"
        );
        return row.SingleOrDefault() ?? null;
    }

    public async Task<ItemModel?> UpdateAsync(ShoppingListModel entity)
    {
        const string sql = """
                           UPDATE ShoppingLists SET Name = @Name, ModifiedAt = @ModifiedAt WHERE Id = @Id;
                           SELECT * FROM ShoppingLists WHERE Id = @Id;
                           """;
        using var connection = databaseConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync(sql, entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM ShoppingLists WHERE Id = @Id";
        using var connection = databaseConnectionFactory.CreateConnection();
        return await connection.ExecuteAsync(sql, new { Id = id });
    }
}