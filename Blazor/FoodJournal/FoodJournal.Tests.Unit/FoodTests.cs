using System.ComponentModel.DataAnnotations;
using FoodJournal.Models;

namespace FoodJournal.Tests.Unit;

public class FoodTests
{
    private const int MaxNameLength = 255;

    private List<ValidationResult> Validate(object instance)
    {
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, context, results, true);
        return results;
    }

    private ValidationResult? ErrorFor(IEnumerable<ValidationResult> errors, string memberName)
    {
        return errors.SingleOrDefault(e => e.MemberNames.Contains(memberName));
    }

    [Fact]
    public void Food_Validates_WithCorrectModel()
    {
        // Arrange
        var food = new Food
        {
            Id = 1,
            Name = "Test Food",
            Icon = "icons8-kawaii-bread-96.png"
        };

        // Act
        var errors = Validate(food);
        var nameError = ErrorFor(errors, nameof(Food.Name));
        var iconError = ErrorFor(errors, nameof(Food.Icon));

        // Assert
        Assert.Null(nameError);
        Assert.Null(iconError);
    }

    [Fact]
    public void FoodIcon_HasDefaultValue_WhenNoIconIsGiven()
    {
        // Arrange
        var food = new Food
        {
            Id = 1,
            Name = "Test Food"
        };

        // Act
        var errors = Validate(food);
        var iconError = ErrorFor(errors, nameof(Food.Icon));

        // Assert
        Assert.Equal("icons8-grocery-bag-96.png", food.Icon);
        Assert.Null(iconError);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Food_DoesNotValidate_WhenNameIsMissing(string? name)
    {
        // Arrange
        var food = new Food
        {
            Id = 1,
            Icon = "icons8-kawaii-bread-96.png",
            Name = name!
        };

        // Act
        var errors = Validate(food);
        var nameError = ErrorFor(errors, nameof(Food.Name));

        // Assert
        Assert.NotNull(nameError);
        Assert.Contains(nameof(Food.Name), nameError.MemberNames);
    }

    [Theory]
    [InlineData(MaxNameLength, true)]
    [InlineData(MaxNameLength + 1, false)]
    public void Name_Validates_NameLength_Boundaries(int length, bool shouldBeValid)
    {
        // Arrange
        var food = new Food
        {
            Id = 1,
            Name = new string('a', length)
        };

        // Act
        var errors = Validate(food);
        var nameError = ErrorFor(errors, nameof(Food.Name));

        // Assert
        if (shouldBeValid)
        {
            Assert.Null(nameError);
        }
        else
        {
            Assert.NotNull(nameError);
            Assert.Contains(nameof(Food.Name), nameError.MemberNames);
        }
    }
}