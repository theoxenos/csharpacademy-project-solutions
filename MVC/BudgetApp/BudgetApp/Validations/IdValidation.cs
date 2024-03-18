using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace BudgetApp.Validations;

public class IdValidation : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value is > 0;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return IsValid(value)
            ? ValidationResult.Success
            : new ValidationResult($"{validationContext.DisplayName} is required");
    }
}