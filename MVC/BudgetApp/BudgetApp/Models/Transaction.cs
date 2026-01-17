using System.ComponentModel.DataAnnotations;
using BudgetApp.Validations;

namespace BudgetApp.Models;

public class Transaction
{
    public int Id { get; init; }

    [StringLength(400, MinimumLength = 3)]
    [Required]
    public string Comment { get; init; } = null!;

    [DataType(DataType.Currency)]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be more than 0")]
    public decimal Amount { get; init; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime Date { get; init; } = DateTime.Now;

    [Required]
    [IdValidation(ErrorMessage = "The Category field is required")]
    public int CategoryId { get; init; }

    public Category Category { get; init; } = null!;
}
