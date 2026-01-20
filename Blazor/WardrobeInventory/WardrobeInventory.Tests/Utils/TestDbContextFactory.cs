using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WardrobeInventory.Blazor.Data;

namespace WardrobeInventory.Tests.Utils;

// Source - https://stackoverflow.com/questions/66101618/moq-idbcontextfactory-with-in-memory-ef-core
// Posted by poke
// Retrieved 2026-01-20, License - CC BY-SA 4.0
public class TestDbContextFactory : IDbContextFactory<WardrobeContext>
{
    private readonly DbContextOptions<WardrobeContext> _options;

    public TestDbContextFactory(DbContextOptions<WardrobeContext> options)
    {
        _options = options;
    }

    public TestDbContextFactory(string? databaseName = null)
    {
        _options = new DbContextOptionsBuilder<WardrobeContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;
    }

    public WardrobeContext CreateDbContext() => new(_options);
}