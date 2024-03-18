using System.ComponentModel.DataAnnotations;
using BudgetApp.Validations;

namespace BudgetApp.Models;

public class Transaction
{
    public int Id { get; set; }
    [StringLength(400, MinimumLength = 3), Required]
    public string Comment { get; set; } = default!;
    [DataType(DataType.Currency), Range(0.01, double.MaxValue, ErrorMessage = "Amount must be more than 0")]
    public decimal Amount { get; set; }
    [DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime Date { get; set; } = DateTime.Now;
    [Required, IdValidation(ErrorMessage = "The Category field is required")]
    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;
}