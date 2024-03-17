using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Models;

public class Transaction
{
    public int Id { get; set; }
    public string Comment { get; set; } = default!;
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }
    [DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime Date { get; set; } = DateTime.Now;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;
}