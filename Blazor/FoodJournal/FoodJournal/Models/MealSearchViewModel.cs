namespace FoodJournal.Models;

public class MealSearchViewModel
{
    public string SearchTerm { get; set; } = default!;
    public DateTime? Date { get; set; } = null;
    public MealType? MealType { get; set; }
}