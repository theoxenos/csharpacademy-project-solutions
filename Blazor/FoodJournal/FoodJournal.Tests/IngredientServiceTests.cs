using System.Net;
using System.Net.Http.Json;
using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Models.Dto;
using FoodJournal.Blazor.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using static FoodJournal.Blazor.Utils.IngredientNameNormalizer;

namespace FoodJournal.Tests;

public class IngredientServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();

    private void GetIngredientService(out IngredientService ingredientService, out Mock<HttpMessageHandler> handlerMock)
    {
        var ingredients = new List<Dictionary<string, string>>
        {
            new()
            {
                { "idIngredient", "1" },
                { "strIngredient", "Chicken" },
                { "strDescription", "Desc" },
                { "strThumb", "http://image.com" },
                { "strType", "" }
            },
            new()
            {
                { "idIngredient", "2" },
                { "strIngredient", "Beef" },
                { "strDescription", "Desc" },
                { "strThumb", "http://image.com" },
                { "strType", "" }
            },
            new()
            {
                { "idIngredient", "3" },
                { "strIngredient", "Fish" },
                { "strDescription", "Desc" },
                { "strThumb", "http://image.com" },
                { "strType", "" }
            }
        };

        var response = new RecipeResponse(ingredients);

        handlerMock = new Mock<HttpMessageHandler>();

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

        _httpClientFactoryMock
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        ingredientService = new IngredientService(_httpClientFactoryMock.Object, memoryCache);
    }

    [Fact]
    public async Task GetAllIngredientsAsync_ReturnsCorrectIngredients()
    {
        // Arrange
        var ingredient = new Ingredient
        {
            Id = 1,
            Name = "Chicken",
            Description = "Desc",
            ThumbnailUrl = "http://image.com",
            Type = ""
        };

        GetIngredientService(out var ingredientService, out _);

        // Act
        var result = await ingredientService.GetAllIngredientsAsync();

        // Assert
        Assert.Equal(3, result.Count);

        Assert.Equal(ingredient.Id, result[0].Id);
        Assert.Equal(NormaliseKey(ingredient.Name), result[0].Name);
        Assert.Equal(ingredient.Description, result[0].Description);
        Assert.Equal(ingredient.ThumbnailUrl, result[0].ThumbnailUrl);
        Assert.Equal(ingredient.Type, result[0].Type);

        Assert.InRange(result[0].Calories, 500, 5000);
        Assert.InRange(result[0].Carbohydrates, 10, 100);
        Assert.InRange(result[0].Protein, 5, 500);
        Assert.InRange(result[0].Fat, 5, 500);
    }

    [Fact]
    public async Task GetAllIngredientsAsync_WhenCalledTwice_UsesCache_AndHitsHttpOnce()
    {
        // Arrange
        GetIngredientService(out var ingredientService, out var handlerMock);

        // Act
        _ = await ingredientService.GetAllIngredientsAsync();
        _ = await ingredientService.GetAllIngredientsAsync();

        // Assert: cache means the HTTP pipeline should be invoked once
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task GetByNameAsync_WhenCalledWithValidName_ReturnsIngredient()
    {
        // Arrange
        var ingredient = new Ingredient
        {
            Id = 1,
            Name = "chicken",
            Description = "Desc",
            ThumbnailUrl = "http://image.com",
            Type = ""
        };

        GetIngredientService(out var ingredientService, out _);

        // Act
        var result = await ingredientService.GetByNameAsync(NormaliseKey(ingredient.Name));

        // Assert
        Assert.Equal(ingredient.Id, result.Id);
        Assert.Equal(NormaliseKey(ingredient.Name), result.Name);
        Assert.Equal(ingredient.Description, result.Description);
        Assert.Equal(ingredient.ThumbnailUrl, result.ThumbnailUrl);
        Assert.Equal(ingredient.Type, result.Type);
    }

    [Fact]
    public async Task GetByNameAsync_WhenCalledTwice_UsesCache_AndHitsHttpOnce()
    {
        // Arrange
        GetIngredientService(out var ingredientService, out var handlerMock);
        var ingredientName = "chicken";
        var ingredientName2 = "beef";

        // Act
        _ = await ingredientService.GetByNameAsync(ingredientName);
        _ = await ingredientService.GetByNameAsync(ingredientName2);

        // Assert: cache means the HTTP pipeline should be invoked once
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}