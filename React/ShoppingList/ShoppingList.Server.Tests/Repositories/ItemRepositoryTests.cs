using Dapper;
using ShoppingList.Server.Models;
using ShoppingList.Server.Repositories;
using ShoppingList.Server.Tests.Infrastructure;
using Xunit;

namespace ShoppingList.Server.Tests.Repositories;

public class ItemRepositoryTests : IClassFixture<TestDatabaseFixture>, IAsyncLifetime
{
    private readonly TestDatabaseFixture _fixture;
    private readonly ItemRepository _itemRepository;
    private readonly ShoppingListRepository _shoppingListRepository;
    
    private ShoppingListModel _testList;
    private ItemModel _testItem;

    public ItemRepositoryTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        _itemRepository = new ItemRepository(fixture.ConnectionFactory);
        _shoppingListRepository = new ShoppingListRepository(fixture.ConnectionFactory);
    }
    
    public async Task InitializeAsync()
    {
        // Runs before each test - can use async/await properly!
        var now = DateTime.UtcNow;
        _testList = await _shoppingListRepository.AddAsync(
            new ShoppingListModel { Name = "Test List", CreatedAt = now, ModifiedAt = now }
        );
        
        // _testItem = await _itemRepository.AddAsync(new ItemModel
        // {
        //     ShoppingListId = _testList.Id,
        //     Name = "Test Item",
        //     Quantity = 1,
        //     IsChecked = false,
        //     CreatedAt = now
        // });
    }

    public async Task DisposeAsync()
    {
        using var connection = _fixture.ConnectionFactory.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM Items");
        await connection.ExecuteAsync("DELETE FROM ShoppingLists");
    }

    [Fact]
    public async Task AddAsync_ShouldInsertAndReturnEntity()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var item = new ItemModel
        {
            ShoppingListId = _testList.Id,
            Name = "Eggs",
            Quantity = 2,
            IsChecked = false,
            CreatedAt = now
        };
        
        // Act
        var saved = await _itemRepository.AddAsync(item);
        
        // Assert
        Assert.True(saved.Id > 0);
        Assert.Equal(item.Name, saved.Name);
        Assert.Equal(item.Quantity, saved.Quantity);
        Assert.Equal(item.IsChecked, saved.IsChecked);
        Assert.Equal(item.CreatedAt, saved.CreatedAt);
        Assert.Equal(_testList.Id, saved.ShoppingListId);
    }
    
    [Fact]
    public async Task GetByShoppingListIdAsync_ShouldReturnItems()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var item = new ItemModel
        {
            ShoppingListId = _testList.Id,
            Name = "Eggs",
            Quantity = 2,
            IsChecked = false,
            CreatedAt = now
        };
        
        await _itemRepository.AddAsync(item);
        
        // Act
        var fetched = await _itemRepository.GetByShoppingListIdAsync(_testList.Id);
        
        // Assert
        Assert.Single(fetched);
        Assert.Equal(item.Name, fetched[0].Name);
        Assert.Equal(item.Quantity, fetched[0].Quantity);
        Assert.Equal(item.IsChecked, fetched[0].IsChecked);
        Assert.Equal(item.CreatedAt, fetched[0].CreatedAt);
        Assert.Equal(_testList.Id, fetched[0].ShoppingListId);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        
        // Act
        var result = await _itemRepository.GetOneAsync(999999);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturn0_WhenNotFound()
    {
        // Arrange
        
        // Act
        var result = await _itemRepository.DeleteAsync(999999);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturn1()
    {
        // Arrange
        var toDeleteItem = await _itemRepository.AddAsync(new ItemModel {ShoppingListId = _testList.Id, Name = "Test Item", Quantity = 1, IsChecked = false});
        
        // Act
        var result = await _itemRepository.DeleteAsync(toDeleteItem.Id);
        var items = await _itemRepository.GetAllAsync();
        
        // Assert
        Assert.Equal(1, result);
        Assert.Empty(items);
    }
}