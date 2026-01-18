using System.ComponentModel.DataAnnotations;
using BudgetApp.Models;

namespace BudgetApp.Tests.UnitTests;

public class CategoryTests
{
    [Fact]
    public void Category_ShouldHaveDefaultColor()
    {
        // Arrange & Act
        var category = new Category { Name = "Test" };

        // Assert
        Assert.Equal("#6c757d", category.Color);
    }

    [Fact]
    public void Category_Validation_ShouldSucceed_WhenModelIsValid()
    {
        // Arrange
        var category = new Category { Name = "Test", Color = "#ffffff" };
        var context = new ValidationContext(category);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(category, context, results, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Category_Validation_ShouldFail_WhenNameIsMissing()
    {
        // Arrange
        var category = new Category { Name = null! };
        var context = new ValidationContext(category);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(category, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Name"));
    }

    [Theory]
    [InlineData("ffffff")] // Valid hex colour but missing #
    [InlineData("#1234567")] // Too long
    [InlineData("#12345")] // Too short
    [InlineData("xss:expression(alert(1))")] // Injection attempt
    [InlineData("#GGGGGG")] // Invalid hex characters
    public void Category_Validation_ShouldFail_WhenColourIsInvalid(string invalidColour)
    {
        // Arrange
        var category = new Category { Name = "Test", Color = invalidColour };
        var context = new ValidationContext(category);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(category, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Color"));
    }

    [Theory]
    [InlineData("#ABC")]
    [InlineData("#123456")]
    [InlineData("#ffffff")]
    public void Category_Validation_ShouldSucceed_WhenColourIsValid(string validColour)
    {
        // Arrange
        var category = new Category { Name = "Test", Color = validColour };
        var context = new ValidationContext(category);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(category, context, results, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Category_Validation_ShouldFail_WhenNameIsTooLong()
    {
        // Arrange
        var category = new Category { Name = new string('a', 256) };
        var context = new ValidationContext(category);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(category, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Name"));
    }

    [Fact]
    public void Category_Transactions_ShouldBeEmptyListByDefault()
    {
        // Arrange & Act
        var category = new Category { Name = "Test" };

        // Assert
        Assert.NotNull(category.Transactions);
        Assert.Empty(category.Transactions);
    }
}