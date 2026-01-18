using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetApp.Models;

public class TransactionUpsertViewModel
{
    public Transaction Transaction { get; init; } = null!;
    public SelectList Categories { get; init; } = null!;
}
