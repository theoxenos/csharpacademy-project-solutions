using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;
using WardrobeInventory.Data;
using WardrobeInventory.Models;

namespace WardrobeInventory.Services;

public class WardrobeService(ISqliteWasmDbContextFactory<WardrobeContext> contextFactory)
{
    public async Task AddItemAsync(WardrobeItem? item)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        context.WardrobeItems.Add(item);
        await context.SaveChangesAsync();
    }

    public async Task<List<WardrobeItem?>> GetAllAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        return await context.WardrobeItems.ToListAsync();
    }

    public async Task<WardrobeItem> GetItemByIdAsync(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        return await context.WardrobeItems.FirstAsync(i => i.Id == id);
    }

    public async Task UpdateItemAsync(WardrobeItem wardrobeItem)
    {
        throw new NotImplementedException();
    }
}