using System.ComponentModel.DataAnnotations;

namespace FoodJournal.Models;

public class Meal
{
    public int Id { get; set; }
    [StringLength(255)]
    public string Name { get; set; } = default!;
    public MealType MealType { get; set; }
    public List<Food> Foods { get; set; } = default!;
}

public enum MealType
{
    Breakfast, 
    Lunch, 
    Dinner, 
    Snack
}