namespace FoodJournal.Blazor.Models.Data;

public record Recipe
{
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public List<Ingredient> Ingredients { get; set; } = [];
}