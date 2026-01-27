using System.ComponentModel.DataAnnotations;

namespace FoodJournal.Models;

public class Food
{
    public int Id { get; init; }

    [StringLength(255)] public string Name { get; init; } = string.Empty;

    [StringLength(100)] public string Icon { get; init; } = "icons8-grocery-bag-96.png";

    public List<Meal> Meals { get; init; } = [];
}