using Microsoft.EntityFrameworkCore;
using WardrobeInventory.Blazor.Models;

namespace WardrobeInventory.Blazor.Data;

public class WardrobeContext(DbContextOptions contextOptions) : DbContext(contextOptions)
{
    public DbSet<WardrobeItem> WardrobeItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<WardrobeItem>().HasData(
            new WardrobeItem { Id = 1, Name = "Shirt", Brand = "BrandA", Category = Category.Shirts, Size = Size.S},
            new WardrobeItem { Id = 2, Name = "Pants", Brand = "BrandB", Category = Category.Pants, Size = Size.M},
            new WardrobeItem { Id = 3, Name = "Shoes", Brand = "BrandC", Category = Category.Shoes, Size = Size.L},
            new WardrobeItem { Id = 4, Name = "Dress", Brand = "BrandD", Category = Category.Dress, Size = Size.XL}
        );
    }
}