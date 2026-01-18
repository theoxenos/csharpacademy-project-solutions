namespace BudgetApp.Models;

public class TransactionIndexViewModel
{
    public Transaction[] Transactions { get; init; } = [];
    public TransactionUpsertViewModel TransactionUpsertViewModel { get; init; } = null!;
    public DateTime? DateFilter { get; init; }
    public string TransactionFilter { get; init; } = string.Empty;
    public IEnumerable<Category> Categories { get; init; } = [];
    public int CategoryFilter { get; init; }
}
