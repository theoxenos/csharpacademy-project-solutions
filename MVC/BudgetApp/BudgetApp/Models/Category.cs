using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Models;

public class Category
{
    public int Id { get; init; }

    [StringLength(255)]
    [DisplayName("Category Name")]
    [Required]
    public string Name { get; init; } = null!;

    [StringLength(7)]
    [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Invalid hex color code")]
    public string Color { get; init; } = "#6c757d";

    public List<Transaction> Transactions { get; init; } = [];
}
