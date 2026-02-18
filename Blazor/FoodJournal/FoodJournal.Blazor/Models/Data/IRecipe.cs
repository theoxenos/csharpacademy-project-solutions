namespace FoodJournal.Blazor.Models.Data;

public interface IRecipe
{
    int Id { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    string ThumbnailUrl { get; set; }
    List<RecipeIngredient> Ingredients { get; set; }
    float TotalCalories { get; }
    float TotalCarbs { get; }
    float TotalProtein { get; }
    float TotalFat { get; }
}