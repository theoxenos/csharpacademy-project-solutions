namespace FoodJournal.Models;

public class MealSearchViewModel
{
    public string SearchTerm { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public MealType? MealType { get; set; }
}