using System.ComponentModel.DataAnnotations;

namespace WardrobeInventory.Models;

public class WardrobeItem
{
    public int Id { get; set; }
    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(200)]
    public string Brand { get; set; } = string.Empty;
    public Category Category { get; set; }
    public Size Size { get; set; }
    public byte[] ImageData { get; set; } = default!;
}

public enum Category
{
    Shirts,
    Pants,
    Dress,
    Shoes
}

public enum Size
{
    S,
    M,
    L
}