namespace BudgetApp.Data;

public class Transaction
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;
}