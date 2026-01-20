using System.ComponentModel.DataAnnotations;
using WardrobeInventory.Blazor.Models;

namespace WardrobeInventory.Tests.UnitTests;

public class WardrobeItemTests
{
    [Fact]
    public void ModelValidation_Should_Pass_WithValidModel()
    {
        // Arrange & Act & Assert
        var item = new WardrobeItem
            { Name = "Test Name", Brand = "Test Brand", Category = Category.Shirts, Size = Size.S };
        var result = new List<ValidationResult>();
        Validator.TryValidateObject(item, new ValidationContext(item), result, true);
        Assert.True(result.Count == 0, $"Validation failed with {result.Count} errors");
    }

    [Theory]
    [InlineData(200)] // Exactly at limit
    [InlineData(199)] // Just under limit
    [InlineData(1)] // Minimum valid
    public void ModelValidation_Should_Pass_WithValidStringLengths(int length)
    {
        // Arrange
        var validString = new string('A', length);
        var item = new WardrobeItem
        {
            Name = validString,
            Brand = validString,
            Category = Category.Shirts,
            Size = Size.S
        };

        // Act
        var result = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(item, new ValidationContext(item), result, true);

        // Assert
        Assert.True(isValid, $"Validation should pass for {length} characters");
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(0)] // Empty string
    [InlineData(201)] // Just over limit
    [InlineData(300)] // Well over limit
    public void ModelValidation_Should_Fail_WhenStringNotRightLength(int length)
    {
        // Arrange
        var notValidString = new string('A', length);
        var item = new WardrobeItem
        {
            Name = notValidString,
            Brand = notValidString,
            Category = Category.Shirts,
            Size = Size.S
        };

        // Act
        var result = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(item, new ValidationContext(item), result, true);

        // Assert
        Assert.False(isValid);
        Assert.NotEmpty(result);
        Assert.Contains(result, r => r.MemberNames.Contains("Name"));
    }
    
    [Theory]
    [InlineData(Category.Shirts)]
    [InlineData(Category.Pants)]
    [InlineData(Category.Dress)]
    [InlineData(Category.Shoes)]
    public void ModelValidation_Should_Pass_WithAllCategoryEnumValues(Category category)
    {
        // Arrange
        var item = new WardrobeItem
        {
            Name = "Test Name",
            Brand = "Test Brand",
            Category = category,
            Size = Size.S
        };

        // Act
        var result = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(item, new ValidationContext(item), result, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(Size.S)]
    [InlineData(Size.M)]
    [InlineData(Size.L)]
    [InlineData(Size.XL)]
    public void ModelValidation_Should_Pass_WithAllSizeEnumValues(Size size)
    {
        // Arrange
        var item = new WardrobeItem
        {
            Name = "Test Name",
            Brand = "Test Brand",
            Category = Category.Shirts,
            Size = size
        };

        // Act
        var result = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(item, new ValidationContext(item), result, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(result);
    }
}