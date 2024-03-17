namespace BudgetApp.Models;

public class TransactionIndexViewModel
{
    public Transaction[] Transactions { get; set; } = [];
    public TransactionUpsertViewModel TransactionUpsertViewModel { get; set; } = default!;
}