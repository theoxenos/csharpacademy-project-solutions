using System.ComponentModel.DataAnnotations;

namespace FoodJournal.Models;

public class Food
{
    public int Id { get; set; }
    [StringLength(255)]
    public string Name { get; set; } = default!;
    public List<Meal> Meals { get; set; } = default!;
}