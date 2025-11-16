using ShoppingList.Server.Models;
using ShoppingList.Server.Repositories;
using ShoppingList.Server.Tests.Infrastructure;
using Xunit;

namespace ShoppingList.Server.Tests.Repositories;

public class ShoppingListRepositoryTests(TestDatabaseFixture fixture) : IClassFixture<TestDatabaseFixture>
{
    [Fact]
    public async Task AddAsync_ShouldInsertAndReturnEntity()
    {
        // Arrange
        var repo = new ShoppingListRepository(fixture.ConnectionFactory);
        var now = DateTime.UtcNow;
        var list = new ShoppingListModel
        {
            Name = "Groceries",
            CreatedAt = now,
            ModifiedAt = now
        };

        // Act
        var saved = await repo.AddAsync(list);

        // Assert
        Assert.True(saved.Id > 0);
        Assert.Equal("Groceries", saved.Name);
        Assert.Equal(list.CreatedAt, saved.CreatedAt);
        Assert.Equal(saved.CreatedAt, saved.ModifiedAt); // insert sets ModifiedAt = CreatedAt
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllInserted()
    {
        // Arrange
        var repo = new ShoppingListRepository(fixture.ConnectionFactory);
        var now = DateTime.UtcNow;

        await repo.AddAsync(new ShoppingListModel { Name = "List A", CreatedAt = now, ModifiedAt = now });
        await repo.AddAsync(new ShoppingListModel { Name = "List B", CreatedAt = now, ModifiedAt = now });

        // Act
        var all = await repo.GetAllAsync();

        // Assert
        Assert.True(all.Count >= 2);
        Assert.Contains(all, l => l.Name == "List A");
        Assert.Contains(all, l => l.Name == "List B");
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var repo = new ShoppingListRepository(fixture.ConnectionFactory);

        // Act
        var result = await repo.GetOneAsync(999999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnEntity_WhenExists()
    {
        // Arrange
        var repo = new ShoppingListRepository(fixture.ConnectionFactory);
        var now = DateTime.UtcNow;
        var created = await repo.AddAsync(new ShoppingListModel { Name = "Find Me", CreatedAt = now, ModifiedAt = now });

        // Act
        var fetched = await repo.GetOneAsync(created.Id);

        // Assert
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched!.Id);
        Assert.Equal("Find Me", fetched.Name);
    }
}
