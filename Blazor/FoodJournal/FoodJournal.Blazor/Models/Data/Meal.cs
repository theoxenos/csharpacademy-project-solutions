namespace FoodJournal.Blazor.Models.Data;

public class Meal
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public MealType Type { get; set; } = MealType.Breakfast;
    public DateTime Date { get; set; } = DateTime.Now;
    public List<Ingredient> Ingredients { get; set; } = [];
}

public enum MealType
{
    Breakfast, Lunch, Dinner, Snack
}