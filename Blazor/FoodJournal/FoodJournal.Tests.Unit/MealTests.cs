using System.ComponentModel.DataAnnotations;
using FoodJournal.Models;

namespace FoodJournal.Tests.Unit;

public class MealTests
{
    private const int MaxNameLength = 255;
    private const int MinFoods = 1;
    private const int MaxFoods = 100;

    private Meal CreateValidMeal(List<Food>? foods = null)
    {
        return new Meal
        {
            Id = 1,
            Date = DateTime.Today,
            MealType = MealType.Breakfast,
            Name = "Test Meal",
            Foods = foods ?? [new Food { Id = 1 }]
        };
    }

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
    public void Meal_Validates_WhenModelIsValid()
    {
        // Arrange
        var meal = CreateValidMeal();

        // Act
        var errors = Validate(meal);

        // Assert
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData(MinFoods, true)]
    [InlineData(MaxFoods, true)]
    [InlineData(0, false)]
    [InlineData(MaxFoods + 1, false)]
    public void Meal_Validates_FoodsCount_Boundaries(int foodsCount, bool shouldBeValid)
    {
        // Arrange
        var meal = CreateValidMeal(Enumerable.Range(1, foodsCount).Select(i => new Food { Id = i }).ToList());

        // Act
        var errors = Validate(meal);
        var foodsError = ErrorFor(errors, nameof(Meal.Foods));

        // Assert
        if (shouldBeValid)
        {
            Assert.Null(foodsError);
        }
        else
        {
            Assert.NotNull(foodsError);
            Assert.Contains(nameof(Meal.Foods), foodsError!.MemberNames);
            Assert.Contains("Foods must have between 1 and 100 entries.", foodsError.ErrorMessage);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Meal_DoesNotValidate_WhenNameIsMissing(string? name)
    {
        // Arrange
        var meal = CreateValidMeal();
        meal.Name = name!;

        // Act
        var errors = Validate(meal);
        var nameError = ErrorFor(errors, nameof(Meal.Name));

        // Assert
        Assert.NotNull(nameError);
        Assert.Contains(nameof(Meal.Name), nameError.MemberNames);
    }

    [Theory]
    [InlineData(MaxNameLength, true)]
    [InlineData(MaxNameLength + 1, false)]
    public void Meal_Validates_NameLength_Boundaries(int length, bool shouldBeValid)
    {
        // Arrange
        var meal = CreateValidMeal();
        meal.Name = new string('a', length);

        // Act
        var errors = Validate(meal);
        var nameError = ErrorFor(errors, nameof(Meal.Name));

        // Assert
        if (shouldBeValid)
        {
            Assert.Null(nameError);
        }
        else
        {
            Assert.NotNull(nameError);
            Assert.Contains(nameof(Meal.Name), nameError.MemberNames);
        }
    }
}