using Microsoft.EntityFrameworkCore;
using WardrobeInventory.Blazor.Data;
using WardrobeInventory.Blazor.Models;

namespace WardrobeInventory.Blazor.Services;

public class WardrobeService(IDbContextFactory<WardrobeContext> contextFactory)
{
    public async Task AddItemAsync(WardrobeItem item)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.WardrobeItems.Add(item);
        await context.SaveChangesAsync();
    }

    public async Task<List<WardrobeItem>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.WardrobeItems.ToListAsync();
    }

    public async Task<WardrobeItem?> GetItemByIdAsync(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.WardrobeItems.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task UpdateItemAsync(WardrobeItem wardrobeItem)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.WardrobeItems.Update(wardrobeItem);
        await context.SaveChangesAsync();
    }

    public async Task DeleteItemAsync(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var item = await context.WardrobeItems.FindAsync(id);
        if (item == null)
        {
            throw new ArgumentException($"Item with id {id} not found");
        }
        context.WardrobeItems.Remove(item);
        await context.SaveChangesAsync();
    }
}