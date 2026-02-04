namespace FoodJournal.Blazor.Models.Data;

public class RecipeIngredient
{
    public string Measurement { get; set; } = string.Empty;
    public Ingredient Ingredient { get; set; } = null!;
}