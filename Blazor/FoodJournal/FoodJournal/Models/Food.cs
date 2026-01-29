using System.ComponentModel.DataAnnotations;

namespace FoodJournal.Models;

public class Food
{
    public int Id { get; init; }

    [StringLength(255)] [Required] public string Name { get; init; } = string.Empty;

    [StringLength(100)] public string Icon { get; init; } = "icons8-grocery-bag-96.png";

    public decimal Calories { get; init; }

    public decimal Protein { get; init; }

    public decimal Carbohydrates { get; init; }

    public decimal Fat { get; init; }

    public List<Meal> Meals { get; init; } = [];
}