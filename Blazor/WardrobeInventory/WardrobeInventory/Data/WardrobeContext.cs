using Microsoft.EntityFrameworkCore;
using WardrobeInventory.Models;

namespace WardrobeInventory.Data;

public class WardrobeContext(DbContextOptions contextOptions) : DbContext(contextOptions)
{
    public DbSet<WardrobeItem> WardrobeItems { get; set; } = default!;
}