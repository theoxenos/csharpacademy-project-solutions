using System.ComponentModel.DataAnnotations;

namespace HabitLoggerMvc.Validators;

public class IdRequiredAttribute(int minValue) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not int intValue)
        {
            return new ValidationResult($"{validationContext.DisplayName} must be an integer.");
        }

        return intValue > minValue
            ? ValidationResult.Success
            : new ValidationResult(GetErrorMessage(validationContext.DisplayName));
    }

    private string GetErrorMessage(string fieldName) => $"{fieldName} is required.";
}
