using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Models;

public class Category
{
    public int Id { get; set; }
    [StringLength(255), DisplayName("Category Name")]
    public string Name { get; set; } = default!;

    public List<Transaction> Transactions { get; set; } = [];
}