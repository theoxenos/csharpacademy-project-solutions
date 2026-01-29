using System.ComponentModel.DataAnnotations;

namespace FoodJournal.Models;

public class Meal
{
    public int Id { get; init; }

    [StringLength(255)] [Required] public string Name { get; set; } = string.Empty;

    public MealType MealType { get; set; }

    [Length(1, 100, ErrorMessage = "Foods must have between 1 and 100 entries.")]
    public List<Food> Foods { get; init; } = [];

    public DateTime Date { get; set; } = DateTime.Today;
}

public enum MealType
{
    Breakfast,
    Lunch,
    Dinner,
    Snack
}