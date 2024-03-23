namespace WardrobeInventory.Models;

public class WardrobeItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
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