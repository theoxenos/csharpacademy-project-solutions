namespace FoodJournal.Blazor.Models.Data;

public record Recipe : IRecipe
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public List<RecipeIngredient> Ingredients { get; set; } = [];
    public float TotalCalories => Ingredients.Sum(i => i.Ingredient.Calories);
    public float TotalCarbs => Ingredients.Sum(i => i.Ingredient.Carbohydrates);
    public float TotalProtein => Ingredients.Sum(i => i.Ingredient.Protein);
    public float TotalFat => Ingredients.Sum(i => i.Ingredient.Fat);
}