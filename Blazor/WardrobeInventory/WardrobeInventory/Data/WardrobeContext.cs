using Microsoft.EntityFrameworkCore;
using WardrobeInventory.Model;

namespace WardrobeInventory.Data;

public class WardrobeContext(DbContextOptions options, HttpClient httpClient) : DbContext(options)
{
    public DbSet<WardrobeItem> WardrobeItems { get; set; } = default!;
}