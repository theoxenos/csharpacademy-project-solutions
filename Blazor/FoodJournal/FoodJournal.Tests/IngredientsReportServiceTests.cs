using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Repositories;
using FoodJournal.Blazor.Services;
using Moq;

namespace FoodJournal.Tests;

public class IngredientsReportServiceTests
{
    [Fact]
    public async Task GetMostUsedIngredientsAsync_ReturnsCorrectIngredients()
    {
        // Arrange
        List<string> ingredientNames = ["egg", "milk", "sugar"];
        var ingredients = ingredientNames.Select(name => new Ingredient { Name = name }).ToList();

        var ingredientRepositoryMock = new Mock<IIngredientsRepository>();
        ingredientRepositoryMock
            .Setup(r => r.GetMostUsedIngredientsName(It.IsAny<int>()))
            .ReturnsAsync(ingredientNames);

        var ingredientServiceMock = new Mock<IIngredientService>();
        ingredientServiceMock
            .Setup(i => i.GetByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string name) => ingredients.First(i => i.Name == name));

        var service = new IngredientsReportsService(ingredientRepositoryMock.Object, ingredientServiceMock.Object);

        // Act
        var result = await service.GetMostUsedIngredientsAsync(3);

        // Assert
        Assert.Equal(ingredients, result);
        ingredientRepositoryMock.Verify(r => r.GetMostUsedIngredientsName(It.IsAny<int>()), Times.Once());
        ingredientServiceMock.Verify(i => i.GetByNameAsync(It.IsAny<string>()), Times.Exactly(ingredientNames.Count));
    }
}