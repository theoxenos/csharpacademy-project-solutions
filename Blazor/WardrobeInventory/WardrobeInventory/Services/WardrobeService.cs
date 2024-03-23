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

    public async Task<List<WardrobeItem>> GetAllAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        return await context.WardrobeItems.ToListAsync();
    }

    public async Task<WardrobeItem> GetItemByIdAsync(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        return await context.WardrobeItems.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task UpdateItemAsync(WardrobeItem wardrobeItem)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.WardrobeItems.Update(wardrobeItem);
        await context.SaveChangesAsync();
    }

    public async Task DeleteItem(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var item = await context.WardrobeItems.FindAsync(id);
        context.WardrobeItems.Remove(item);
        await context.SaveChangesAsync();
    }
}