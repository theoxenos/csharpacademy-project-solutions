using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Validations;

public class IdValidation : ValidationAttribute
{
    public override bool IsValid(object? value) => value is > 0;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
        IsValid(value)
            ? ValidationResult.Success
            : new ValidationResult($"{validationContext.DisplayName} is required");
}
