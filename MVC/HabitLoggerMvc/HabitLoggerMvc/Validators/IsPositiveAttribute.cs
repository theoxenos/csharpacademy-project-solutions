using System.ComponentModel.DataAnnotations;

namespace HabitLoggerMvc.Validators;

public class IsPositiveAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) => value is >= 0;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (IsValid(value))
        {
            return ValidationResult.Success;
        }

        string errorMessage = FormatErrorMessage(validationContext.DisplayName);
        return new ValidationResult(errorMessage);
    }

    public override string FormatErrorMessage(string name) =>
        // Customize the error message here
        $"{name} must not be negative.";
}
