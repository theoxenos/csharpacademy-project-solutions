using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Data;

public class Category
{
    public int Id { get; set; }
    [StringLength(255)]
    public string Name { get; set; } = default!;

    public List<Transaction> Transactions { get; set; } = [];
}