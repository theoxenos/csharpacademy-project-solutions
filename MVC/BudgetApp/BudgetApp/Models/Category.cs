using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Models;

public class Category
{
    public int Id { get; set; }
    [StringLength(255), DisplayName("Category Name"), Required]
    public string Name { get; set; } = default!;
    [StringLength(7), RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Invalid hex color code")]
    public string Color { get; set; } = "#6c757d";

    public List<Transaction> Transactions { get; set; } = [];
}