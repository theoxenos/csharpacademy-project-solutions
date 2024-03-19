using Microsoft.EntityFrameworkCore;
using WardrobeInventory.Model;

namespace WardrobeInventory.Data;

public class WardrobeContext(DbContextOptions options, HttpClient httpClient) : DbContext(options)
{
    public DbSet<WardrobeItem> WardrobeItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WardrobeItem>().HasData(
            new WardrobeItem
            {
                Id = 1,
                Name = "My favourite shirt",
                Category = Category.Shirts,
                Brand = "Grandma",
                Size = Size.M,
                ImageData = ConvertImageToBytes("item1.png").Result
            },
            new WardrobeItem
            {
                Id = 2,
                Name = "My favourite coat",
                Category = Category.Dress,
                Brand = "Prada",
                Size = Size.L,
                ImageData = ConvertImageToBytes("item2.png").Result
            }
        );
    }

    // private byte[] ConvertImageToBytes(string imageName)
    // {
    //     var imagePath = $"./wwwroot/sample-images/{imageName}";
    //
    //
    //
    //     return File.ReadAllBytes(imagePath);
    // }
    
    private async Task<byte[]> ConvertImageToBytes(string imageName)
    {
        var imagePath = Path.Combine("wwwroot", "sample-images", imageName);
    
        // var image = 
        
        return await httpClient.GetByteArrayAsync($"sample-images/{imageName}");
    }
}