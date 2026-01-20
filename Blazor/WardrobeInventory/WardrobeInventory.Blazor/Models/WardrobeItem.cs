using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WardrobeInventory.Blazor.Models;

public class WardrobeItem
{
    public int Id { get; init; }
    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(200)]
    public string Brand { get; set; } = string.Empty;
    public Category Category { get; set; }
    public Size Size { get; set; }
    public byte[]? ImageData { get; set; }
}

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum Category
{
    Shirts,
    Pants,
    Dress,
    Shoes
}

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum Size
{
    S,
    M,
    L
}