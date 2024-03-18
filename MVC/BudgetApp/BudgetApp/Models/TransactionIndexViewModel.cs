namespace BudgetApp.Models;

public class TransactionIndexViewModel
{
    public Transaction[] Transactions { get; set; } = [];
    public TransactionUpsertViewModel TransactionUpsertViewModel { get; set; } = default!;
    public DateTime? DateFilter { get; set; }
    public string TransactionFilter { get; set; } = string.Empty;
    public IEnumerable<Category> Categories { get; set; } = [];
    public int CategoryFilter { get; set; } = default!;
}