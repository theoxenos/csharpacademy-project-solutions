using Microsoft.EntityFrameworkCore;
using WardrobeInventory.Blazor.Models;

namespace WardrobeInventory.Blazor.Data;

public class WardrobeContext(DbContextOptions contextOptions) : DbContext(contextOptions)
{
    public DbSet<WardrobeItem> WardrobeItems { get; set; } = null!;
}