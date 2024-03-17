using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetApp.Models;

public class TransactionUpsertViewModel
{
    public Transaction Transaction { get; set; } = default!;
    public SelectList Categories { get; set; } = default!;
}