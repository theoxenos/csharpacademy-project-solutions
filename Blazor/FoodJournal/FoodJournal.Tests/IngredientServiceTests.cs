using System.Net;
using System.Net.Http.Json;
using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Models.Dto;
using FoodJournal.Blazor.Services;
using Moq;
using Moq.Protected;
using Xunit;
using static FoodJournal.Blazor.Utils.IngredientNameNormalizer;

namespace FoodJournal.Tests;

public class IngredientServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();

    [Fact]
    public async Task GetAllIngredientsAsync_ReturnsCorrectIngredients()
    {
        // Arrange
        var ingredient = new Ingredient
        {
            Id = 1,
            Name = "Chicken",
            Description = "The chicken is a type of domesticated fowl",
            ThumbnailUrl = "http://image.com",
            Type = null!
        };
        var ingredients = new Dictionary<string, string>
        {
            { "idIngredient", ingredient.Id.ToString() },
            { "strIngredient", ingredient.Name },
            { "strDescription", ingredient.Description },
            { "strThumb", ingredient.ThumbnailUrl },
            { "strType", ingredient.Type }
        };

        var response = new RecipeResponse([ingredients]);

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
        
        var ingredientService = new IngredientService(_httpClientFactoryMock.Object);
        
        // Act
        var result = await ingredientService.GetAllIngredientsAsync();
            
        // Assert
        Assert.Single(result);
        
        Assert.Equal(ingredient.Id, result[0].Id);
        Assert.Equal(NormaliseKey(ingredient.Name), result[0].Name);
        Assert.Equal(ingredient.Description, result[0].Description);
        Assert.Equal(ingredient.ThumbnailUrl, result[0].ThumbnailUrl);
        Assert.Null(result[0].Type);
        
        Assert.InRange(result[0].Calories, 500, 5000);
        Assert.InRange(result[0].Carbohydrates, 10, 100);
        Assert.InRange(result[0].Protein, 5, 500);
        Assert.InRange(result[0].Fat, 5, 500);
    }
}