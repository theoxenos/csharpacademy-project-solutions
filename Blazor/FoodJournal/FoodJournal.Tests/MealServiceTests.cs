using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Repositories;
using FoodJournal.Blazor.Services;
using Moq;

namespace FoodJournal.Tests;

public class MealServiceTests
{
    private List<Meal> GetTestMealsForAdding()
    {
        return
        [
            new Meal
            {
                Id = 1,
                Name = "Test meal 1",
                Date = new DateTime(2026, 1, 15, 12, 0, 0),
                Description = "Test description 1",
                ThumbnailUrl = "http://image.com",
                Type = MealType.Breakfast
            },

            new Meal
            {
                Id = 2,
                Name = "Test meal 2",
                Date = new DateTime(2026, 1, 16, 12, 0, 0),
                Description = "Test description 2",
                ThumbnailUrl = "http://image.com",
                Type = MealType.Dinner
            }
        ];
    }

    private List<Meal> GetTestMealsForComparison()
    {
        var meals = GetTestMealsForAdding();
        for (var i = 0; i < meals.Count; i++)
        {
            meals[i].Id = i + 1;
            meals[i].Ingredients =
            [
                new RecipeIngredient
                {
                    Ingredient = new Ingredient { Name = $"Ingredient {i + 1}" },
                    Measurement = "100g"
                }
            ];
        }

        return meals;
    }

    private (MealService Service, Mock<IMealRepository> MealRepo) CreateMockServices(
        Dictionary<int, List<(string Name, string Measurement)>>? ingredientsByMealId = null)
    {
        var ingredientsRepositoryMock = new Mock<IIngredientsRepository>();
        ingredientsRepositoryMock
            .Setup(r => r.GetAllIngredientsByMealIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int mealId) => ingredientsByMealId?[mealId] ?? []);

        var ingredientServiceMock = new Mock<IIngredientService>();
        ingredientServiceMock
            .Setup(s => s.GetByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string name) => new Ingredient { Name = name });

        var mealRepositoryMock = new Mock<IMealRepository>();

        var service = new MealService(
            ingredientServiceMock.Object,
            ingredientsRepositoryMock.Object,
            mealRepositoryMock.Object);

        return (service, mealRepositoryMock);
    }

    [Fact]
    public async Task AddMealAsync_ReturnsCorrectMeal()
    {
        // Arrange
        var mealToAdd = GetTestMealsForAdding()[0];

        var expectedIngredients = mealToAdd.Ingredients.Select(i =>
            (i.Ingredient.Name, i.Measurement)).ToList();

        var ingredientsByMealId = new Dictionary<int, List<(string Name, string Measurement)>>
        {
            { 1, expectedIngredients }
        };

        var (mealService, mealRepositoryMock) = CreateMockServices(ingredientsByMealId);
        mealRepositoryMock.Setup(r => r.AddMealAsync(It.IsAny<Meal>())).ReturnsAsync(1);

        // Act
        var result = await mealService.AddMealAsync(mealToAdd);

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal(mealToAdd.Name, result.Name);
        Assert.Equal(mealToAdd.Date, result.Date);
        Assert.Equal(mealToAdd.Description, result.Description);
        Assert.Equal(mealToAdd.ThumbnailUrl, result.ThumbnailUrl);
        Assert.Equal(mealToAdd.Type, result.Type);
        Assert.Equal(expectedIngredients.Count, result.Ingredients.Count);
        for (var i = 0; i < expectedIngredients.Count; i++)
        {
            Assert.Equal(expectedIngredients[i].Name, result.Ingredients[i].Ingredient.Name);
            Assert.Equal(expectedIngredients[i].Measurement, result.Ingredients[i].Measurement);
        }
    }

    [Fact]
    public async Task GetAllMealsAsync_ReturnsCorrectMeals()
    {
        // Arrange
        var meals = GetTestMealsForAdding();

        var ingredientsByMealId = meals.ToDictionary(
            m => m.Id,
            m => m.Ingredients.Select(i => (i.Ingredient.Name, i.Measurement)).ToList());

        var (mealService, mealRepositoryMock) = CreateMockServices(ingredientsByMealId);
        mealRepositoryMock.Setup(r => r.GetAllMealsAsync()).ReturnsAsync(meals);

        // Act
        var result = await mealService.GetAllMealsAsync();

        // Assert
        Assert.Equal(meals, result);
    }

    [Fact]
    public async Task GetAllMealsAsync_ReturnsEmptyList_WhenNoMealsInDatabase()
    {
        // Arrange
        var (mealService, mealRepositoryMock) = CreateMockServices();
        mealRepositoryMock.Setup(r => r.GetAllMealsAsync()).ReturnsAsync(new List<Meal>());

        // Act
        var result = await mealService.GetAllMealsAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMealByIdAsync_ReturnsCorrectMeal()
    {
        // Arrange
        var meal = GetTestMealsForComparison()[0];

        var (mealService, mealRepositoryMock) = CreateMockServices();
        mealRepositoryMock.Setup(r => r.GetMealByIdAsync(It.IsAny<int>())).ReturnsAsync(meal);

        // Act
        var result = await mealService.GetMealByIdAsync(meal.Id);

        // Assert
        Assert.Equal(meal, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(420)]
    public async Task GetMealByIdAsync_ReturnsNull_WhenMealNotFound(int? mealId)
    {
        // Arrange
        var (mealService, mealRepositoryMock) = CreateMockServices();
        mealRepositoryMock.Setup(r => r.GetMealByIdAsync(It.IsAny<int>())).ReturnsAsync((Meal?)null);

        // Act
        var result = await mealService.GetMealByIdAsync(mealId ?? 0);

        // Assert
        Assert.Null(result);
    }
}