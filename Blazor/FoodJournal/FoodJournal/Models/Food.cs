using System.ComponentModel.DataAnnotations;

namespace FoodJournal.Models;

public class Food
{
    public int Id { get; set; }
    [StringLength(255)]
    public string Name { get; set; } = default!;
    [StringLength(100)]
    public string Icon { get; set; } = "icons8-grocery-bag-96.png";
    public List<Meal> Meals { get; set; } = default!;
}