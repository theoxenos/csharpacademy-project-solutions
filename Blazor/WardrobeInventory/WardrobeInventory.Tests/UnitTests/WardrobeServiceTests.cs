using Microsoft.EntityFrameworkCore;
using WardrobeInventory.Blazor.Data;
using WardrobeInventory.Blazor.Models;
using WardrobeInventory.Blazor.Services;
using WardrobeInventory.Tests.Utils;

namespace WardrobeInventory.Tests.UnitTests;

public class WardrobeServiceTests
{
    private async Task<(WardrobeService wardrobeService, WardrobeItem wardrobeItem)> CreateServiceObjectWithTestItem(TestDbContextFactory factory)
    {
        var wardrobeService = new WardrobeService(factory);

        var wardrobeItem = new WardrobeItem
        {
            Brand = "Test Brand",
            Name = "Test Item",
            Category = Category.Shoes,
            Size = Size.S
        };
        
        await wardrobeService.AddItemAsync(wardrobeItem);
        return (wardrobeService, wardrobeItem);
    }

    private TestDbContextFactory CreateTestFactoryObject()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<WardrobeContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return new TestDbContextFactory(options);
    }
    
    [Fact]
    public async Task AddItemAsync_Adds_Item_To_Database()
    {
        // Arrange
        var factory = CreateTestFactoryObject();
        var wardrobeItem = new WardrobeItem
        {
            Brand = "Test Brand",
            Name = "Test Item",
            Category = Category.Shoes,
            Size = Size.S
        };
        var wardrobeService = new WardrobeService(factory);

        // Act
        await wardrobeService.AddItemAsync(wardrobeItem);

        // Assert (using a separate context with the same options)
        await using var assertContext = factory.CreateDbContext();
        var addedItem = await assertContext.WardrobeItems.FindAsync(wardrobeItem.Id);
        Assert.NotNull(addedItem);
        Assert.Equal(wardrobeItem.Brand, addedItem.Brand);
        Assert.Equal(wardrobeItem.Name, addedItem.Name);
        Assert.Equal(wardrobeItem.Category, addedItem.Category);
        Assert.Equal(wardrobeItem.Size, addedItem.Size);

        var totalItems = await assertContext.WardrobeItems.CountAsync();
        Assert.Equal(1, totalItems);
    }

    [Fact]
    public async Task AddItemAsync_Throws_When_Item_Is_Null()
    {
        // Arrange
        var wardrobeService = new WardrobeService(new TestDbContextFactory());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => wardrobeService.AddItemAsync(null!));
    }

    [Fact]
    public async Task GetAllAsync_Returns_Correct_Items_And_Amount()
    {
        // Arrange
        var factory = CreateTestFactoryObject();
        var (wardrobeService, wardrobeItem) = await CreateServiceObjectWithTestItem(factory);

        // Act
        var items = await wardrobeService.GetAllAsync();

        // Assert
        Assert.Single(items);
        Assert.Equal(wardrobeItem.Id, items[0].Id);
        Assert.Equal(wardrobeItem.Brand, items[0].Brand);
        Assert.Equal(wardrobeItem.Name, items[0].Name);
        Assert.Equal(wardrobeItem.Category, items[0].Category);
        Assert.Equal(wardrobeItem.Size, items[0].Size);
    }

    [Fact]
    public async Task GetItemByIdAsync_Returns_Correct_Item()
    {
        // Arrange
        var factory = CreateTestFactoryObject();
        var (wardrobeService, wardrobeItem) = await CreateServiceObjectWithTestItem(factory);

        // Act
        var item = await wardrobeService.GetItemByIdAsync(wardrobeItem.Id);

        // Assert
        Assert.NotNull(item);
        Assert.Equal(wardrobeItem.Id, item.Id);
        Assert.Equal(wardrobeItem.Brand, item.Brand);
        Assert.Equal(wardrobeItem.Name, item.Name);
        Assert.Equal(wardrobeItem.Category, item.Category);
        Assert.Equal(wardrobeItem.Size, item.Size);
    }

    [Fact]
    public async Task GetItemByIdAsync_Returns_Null_With_NonExisting_Id()
    {
        // Arrange
        var wardrobeService = new WardrobeService(new TestDbContextFactory());

        // Act
        var item = await wardrobeService.GetItemByIdAsync(0);

        Assert.Null(item);
    }

    [Fact]
    public async Task UpdateItemAsync_Updates_Correctly()
    {
        // Arrange
        var factory = CreateTestFactoryObject();
        var (wardrobeService, wardrobeItem) = await CreateServiceObjectWithTestItem(factory);
        wardrobeItem.Brand = "Updated Brand";
        
        // Act
        await wardrobeService.UpdateItemAsync(wardrobeItem);
        
        // Assert (using a separate context with the same options)
        await using var assertContext = factory.CreateDbContext();
        var updatedItem = await assertContext.WardrobeItems.FindAsync(wardrobeItem.Id);
        Assert.NotNull(updatedItem);
        Assert.Equal(wardrobeItem.Brand, updatedItem.Brand);
        Assert.Equal(wardrobeItem.Name, updatedItem.Name);
        Assert.Equal(wardrobeItem.Category, updatedItem.Category);
        Assert.Equal(wardrobeItem.Size, updatedItem.Size);
        
        Assert.Single(await assertContext.WardrobeItems.ToArrayAsync());
    }
    
    [Fact]
    public async Task DeleteItemAsync_Deletes_Item_From_Database()
    {
        // Arrange
        var factory = CreateTestFactoryObject();
        var (wardrobeService, wardrobeItem) = await CreateServiceObjectWithTestItem(factory);

        // Act
        await wardrobeService.DeleteItemAsync(wardrobeItem.Id);

        // Assert (using a separate context with the same options)
        await using var assertContext = factory.CreateDbContext();
        var deletedItem = await assertContext.WardrobeItems.FindAsync(wardrobeItem.Id);
        Assert.Null(deletedItem);

        var totalItems = await assertContext.WardrobeItems.CountAsync();
        Assert.Equal(0, totalItems);
    }

    [Fact]
    public async Task DeleteItemAsync_Throws_When_ItemId_Is_Not_Found()
    {
        // Arrange
        var wardrobeService = new WardrobeService(new TestDbContextFactory());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => wardrobeService.DeleteItemAsync(0));
    }

    [Fact]
    public async Task DeleteItemAsync_Does_Not_Delete_When_Item_Does_Not_Exist()
    {
        // Arrange
        var factory = CreateTestFactoryObject();
        var (wardrobeService, wardrobeItem) = await CreateServiceObjectWithTestItem(factory);
        var nonExistingItem = new WardrobeItem
        {
            Id = 9999,
            Brand = "Non Existing",
            Name = "Non Existing Item",
            Category = Category.Shirts,
            Size = Size.M
        };

        // Act
        try
        {
            await wardrobeService.DeleteItemAsync(nonExistingItem.Id);
        }
        catch (Exception)
        {
            // ignored
        }

        // Assert (using a separate context with the same options)
        await using var assertContext = factory.CreateDbContext();
        var existingItem = await assertContext.WardrobeItems.FindAsync(wardrobeItem.Id);
        Assert.NotNull(existingItem);

        var totalItems = await assertContext.WardrobeItems.CountAsync();
        Assert.Equal(1, totalItems);
    }
}