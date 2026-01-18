using System.ComponentModel.DataAnnotations;
using BudgetApp.Models;

namespace BudgetApp.Tests.UnitTests;

public class TransactionTests
{
    [Fact]
    public void Transaction_Validation_ShouldSucceed_WhenModelIsValid()
    {
        // Arrange
        var transaction = new Transaction
        {
            Comment = "Test Transaction",
            Amount = 100.00m,
            CategoryId = 1,
            Category = new Category { Id = 1, Name = "Test" }
        };
        var context = new ValidationContext(transaction);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(transaction, context, results, true);

        // Assert
        Assert.True(isValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ab")] // Too short (min 3)
    [InlineData("a")]
    [InlineData("this comment is definitely more than four hundred characters long... (simulated)")]
    public void Transaction_Validation_ShouldFail_WhenCommentIsInvalid(string? comment)
    {
        // Handle the long string case dynamically for the theory
        if (comment != null && comment.Contains("(simulated)")) comment = new string('a', 401);

        // Arrange
        var transaction = new Transaction
        {
            Comment = comment!,
            Amount = 100.00m,
            CategoryId = 1,
            Category = new Category { Id = 1, Name = "Test" }
        };
        var context = new ValidationContext(transaction);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(transaction, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Comment"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(0.009)]
    public void Transaction_Validation_ShouldFail_WhenAmountIsInvalid(double amount)
    {
        // Arrange
        var transaction = new Transaction
        {
            Comment = "Test Transaction",
            Amount = (decimal)amount,
            CategoryId = 1,
            Category = new Category { Id = 1, Name = "Test" }
        };
        var context = new ValidationContext(transaction);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(transaction, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Amount"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Transaction_Validation_ShouldFail_WhenCategoryIdIsInvalid(int categoryId)
    {
        // Arrange
        var transaction = new Transaction
        {
            Comment = "Test Transaction",
            Amount = 100.00m,
            CategoryId = categoryId,
            Category = new Category { Id = 1, Name = "Test" }
        };
        var context = new ValidationContext(transaction);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(transaction, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.NotEmpty(results);
    }

    [Fact]
    public void Transaction_ShouldHaveCurrentDateByDefault()
    {
        // Arrange & Act
        var transaction = new Transaction();

        // Assert
        Assert.True((DateTime.Now - transaction.Date).TotalSeconds < 5);
    }
}