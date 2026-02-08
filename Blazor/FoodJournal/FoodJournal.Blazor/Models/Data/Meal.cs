namespace FoodJournal.Blazor.Models.Data;

public class Meal
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public MealType Type { get; set; } = MealType.Breakfast;
    public DateTime Date { get; set; } = DateTime.Now;
    public List<RecipeIngredient> Ingredients { get; set; } = [];

    public float TotalCalories => Ingredients.Sum(i => i.Ingredient.Calories);
    public float TotalCarbs => Ingredients.Sum(i => i.Ingredient.Carbohydrates);
    public float TotalProtein => Ingredients.Sum(i => i.Ingredient.Protein);
    public float TotalFat => Ingredients.Sum(i => i.Ingredient.Fat);

    public static Meal FromRecipe(Recipe recipe)
    {
        ArgumentNullException.ThrowIfNull(recipe);

        return new Meal
        {
            Name = recipe.Name,
            Description = recipe.Description,
            Ingredients = recipe.Ingredients
        };
    }
}

public enum MealType
{
    Breakfast,
    Lunch,
    Dinner,
    Snack
}

public static class MealTypeExtensions
{
    public static string MealTypeToIconUrl(this MealType type)
    {
        return $"/images/icons8-{type.ToString().ToLower()}-50.png";
    }
}