using System.Net;
using System.Net.Http.Json;
using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Models.Dto;
using FoodJournal.Blazor.Repositories;
using FoodJournal.Blazor.Services;
using Moq;
using Moq.Protected;
using Xunit;

namespace FoodJournal.Tests;

public class RecipeServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<IIngredientsRepository> _ingredientsRepositoryMock;
    private readonly RecipeService _recipeService;

    public RecipeServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _ingredientsRepositoryMock = new Mock<IIngredientsRepository>();
        _recipeService = new RecipeService(_httpClientFactoryMock.Object, _ingredientsRepositoryMock.Object);
    }

    [Fact]
    public async Task SearchRecipeByNameAsync_WhenMealsFound_ReturnsRecipes()
    {
        // Arrange
        var recipeName = "Chicken";
        var mealData = new Dictionary<string, string>
        {
            { "strMeal", "Chicken Adobo" },
            { "strInstructions", "Cook it" },
            { "strMealThumb", "http://image.com" },
            { "strIngredient1", "Chicken" },
            { "strMeasure1", "1kg" }
        };

        var response = new RecipeResponse([mealData]);
        
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(response)
            });

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _ingredientsRepositoryMock.Setup(x => x.GetByNameAsync("Chicken"))
            .ReturnsAsync(new Ingredient { Name = "Chicken", Calories = 100 });

        // Act
        var result = await _recipeService.SearchRecipeByNameAsync(recipeName);

        // Assert
        Assert.Single(result);
        var recipe = result[0];
        Assert.Equal("Chicken Adobo", recipe.Name);
        Assert.Single(recipe.Ingredients);
        Assert.Equal("Chicken", recipe.Ingredients[0].Ingredient.Name);
        Assert.Equal("1kg", recipe.Ingredients[0].Measurement);
    }

    [Fact]
    public async Task SearchRecipeByNameAsync_WhenNoMealsFound_ReturnsEmptyList()
    {
        // Arrange
        var response = new RecipeResponse(null!);
        
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(response)
            });

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _recipeService.SearchRecipeByNameAsync("Unknown");

        // Assert
        Assert.Empty(result);
    }
}
