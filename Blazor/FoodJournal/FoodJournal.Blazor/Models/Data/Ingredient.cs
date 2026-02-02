namespace FoodJournal.Blazor.Models.Data;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public float Calories { get; set; }
    public float Carbs { get; set; }
    public float Fat { get; set; }
    public float Protein { get; set; }
}