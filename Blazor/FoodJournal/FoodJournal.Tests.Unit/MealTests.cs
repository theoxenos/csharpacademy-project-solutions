using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FoodJournal.Models;

namespace FoodJournal.Tests.Unit;

public class MealTests
{
    private readonly string _foodsErrorMessage;
    private readonly int _maxFoodsLength;
    private readonly int _maxNameLength;
    private readonly string _nameErrorMessage;

    public MealTests()
    {
        var mealNameProp = typeof(Meal).GetProperty(nameof(Meal.Name));
        var stringLengthAttribute = mealNameProp?.GetCustomAttributes<StringLengthAttribute>().Single();
        _maxNameLength = stringLengthAttribute?.MaximumLength ?? 255;
        _nameErrorMessage = stringLengthAttribute?.ErrorMessage ?? "Name must be between 1 and 255 characters.";

        var mealFoodsProp = typeof(Meal).GetProperty(nameof(Meal.Foods));
        var lengthAttribute = mealFoodsProp?.GetCustomAttributes<LengthAttribute>().Single();
        _maxFoodsLength = lengthAttribute?.MaximumLength ?? 100;
        _foodsErrorMessage = lengthAttribute?.ErrorMessage ?? "Foods must have between 1 and 100 entries.";
    }

    [Fact]
    public void Meal_Validates_WithCorrectModel()
    {
        // Arrange
        var food = new Food
        {
            Id = 1,
            Name = "Test Food",
            Icon = "icons8-kawaii-bread-96.png"
        };
        var meal = new Meal
        {
            Id = 1,
            Date = DateTime.Today,
            Foods = [food],
            MealType = MealType.Breakfast,
            Name = "Test Meal"
        };

        // Act
        var validationContext = new ValidationContext(meal);
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(meal, validationContext, errors, true);

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void Meal_Validates_WithFoodsUptoLimit()
    {
        // Arrange
        var meal = new Meal
        {
            Id = 1,
            Date = DateTime.Today,
            Foods = Enumerable.Range(1, _maxFoodsLength).Select(i => new Food { Id = i }).ToList(),
            MealType = MealType.Breakfast,
            Name = "Test Meal"
        };

        // Act
        var validationContext = new ValidationContext(meal);
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(meal, validationContext, errors, true);

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void Meal_DoesntValidate_WhenNoFoodsAdded()
    {
        // Arrange
        var meal = new Meal
        {
            Date = DateTime.Today,
            Id = 1,
            MealType = MealType.Breakfast,
            Name = "Test Meal"
        };

        // Act
        var validationContext = new ValidationContext(meal);
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(meal, validationContext, errors, true);

        // Assert
        Assert.Single(errors);
        Assert.Contains(_foodsErrorMessage, errors[0].ErrorMessage);
        Assert.Contains(nameof(Meal.Foods), errors[0].MemberNames);
    }

    [Fact]
    public void Meal_DoesntValidate_WhenTooManyFoodsAdded()
    {
        // Arrange
        var meal = new Meal
        {
            Date = DateTime.Today,
            Id = 1,
            MealType = MealType.Breakfast,
            Name = "Test Meal",
            Foods = Enumerable.Range(1, _maxFoodsLength + 1).Select(i => new Food { Id = i }).ToList()
        };

        // Act
        var validationContext = new ValidationContext(meal);
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(meal, validationContext, errors, true);

        // Assert
        Assert.Single(errors);
        Assert.Contains(_foodsErrorMessage, errors[0].ErrorMessage);
        Assert.Contains(nameof(Meal.Foods), errors[0].MemberNames);
    }

    [Fact]
    public void Meal_DoesntValidate_WhenNoNameIsGiven()
    {
        // Arrange
        var meal = new Meal
        {
            Date = DateTime.Today,
            Id = 1,
            MealType = MealType.Breakfast,
            Foods = [new Food { Id = 1 }]
        };

        // Act
        var validationContext = new ValidationContext(meal);
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(meal, validationContext, errors, true);

        // Assert
        Assert.Single(errors);
        Assert.Contains(_nameErrorMessage, errors[0].ErrorMessage);
        Assert.Contains(nameof(Meal.Name), errors[0].MemberNames);
    }

    [Fact]
    public void Meal_DoesntValidate_WhenNameIsOverLimit()
    {
        // Arrange
        var meal = new Meal
        {
            Date = DateTime.Today,
            Id = 1,
            MealType = MealType.Breakfast,
            Foods = [new Food { Id = 1 }],
            Name = new string('a', _maxNameLength + 1)
        };

        // Act
        var validationContext = new ValidationContext(meal);
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(meal, validationContext, errors, true);

        // Assert
        Assert.Single(errors);
        Assert.Contains(_nameErrorMessage, errors[0].ErrorMessage);
        Assert.Contains(nameof(Meal.Name), errors[0].MemberNames);
    }
}