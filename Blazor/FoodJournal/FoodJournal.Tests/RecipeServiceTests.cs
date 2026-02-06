using System.Net;
using System.Net.Http.Json;
using FoodJournal.Blazor.Models.Data;
using FoodJournal.Blazor.Models.Dto;
using FoodJournal.Blazor.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;

namespace FoodJournal.Tests;

public class RecipeServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<IIngredientService> _ingredientServiceMock;
    private readonly RecipeService _recipeService;

    public RecipeServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _ingredientServiceMock = new Mock<IIngredientService>();
        var cache = new MemoryCache(new MemoryCacheOptions());
        _recipeService = new RecipeService(_httpClientFactoryMock.Object, _ingredientServiceMock.Object, cache);
    }

    [Fact]
    public async Task SearchRecipeByNameAsync_WhenMealsFound_ReturnsRecipes()
    {
        // Arrange
        var recipeName = "Chicken";
        var mealData = new Dictionary<string, string>
        {
            { "idMeal", "12345" },
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

        _ingredientServiceMock.Setup(x => x.GetByNameAsync("Chicken"))
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

    [Fact]
    public async Task SearchRecipeByNameAsync_MultipleIngredients_DoesNotCallIngredientApiPerIngredient()
    {
        // Arrange: use the REAL IngredientService so caching is tested
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        var handler = new RoutingCountingHandler();
        var httpClient = new HttpClient(handler);

        _httpClientFactoryMock
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        var ingredientService = new IngredientService(_httpClientFactoryMock.Object, memoryCache);
        var recipeService = new RecipeService(_httpClientFactoryMock.Object, ingredientService, memoryCache);

        // Act
        var result = await recipeService.SearchRecipeByNameAsync("anything");

        // Assert
        Assert.Single(result);
        Assert.Equal(3, result[0].Ingredients.Count);

        // Key assertion: ingredient API list endpoint is called ONCE despite 3 ingredients
        Assert.Equal(1, handler.IngredientsListCalls);

        // Sanity: recipes endpoint called once
        Assert.Equal(1, handler.SearchCalls);
    }
}

/// <summary>
///     Provides a mechanism to track HTTP request counts during message handling.
///     This class is intended to monitor and count the calls made to specific API endpoints
///     such as recipe search and ingredient listing.
/// </summary>
file sealed class RoutingCountingHandler : HttpMessageHandler
{
    public int TotalCalls { get; private set; }
    public int SearchCalls { get; private set; }
    public int IngredientsListCalls { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        TotalCalls++;

        var url = request.RequestUri?.ToString() ?? string.Empty;

        if (url.Contains("search.php", StringComparison.OrdinalIgnoreCase))
        {
            SearchCalls++;

            // Recipe with multiple ingredients -> should NOT cause multiple HTTP calls for ingredients
            var mealData = new Dictionary<string, string>
            {
                { "idMeal", "12345" },
                { "strMeal", "Chicken & Beef Plate" },
                { "strInstructions", "Cook it" },
                { "strMealThumb", "http://image.com" },

                { "strIngredient1", "Chicken" },
                { "strMeasure1", "1kg" },

                { "strIngredient2", "Beef" },
                { "strMeasure2", "250g" },

                { "strIngredient3", "Fish" },
                { "strMeasure3", "100g" }
            };

            var recipes = new RecipeResponse([mealData]);

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(recipes)
            });
        }

        if (url.Contains("list.php?i=list", StringComparison.OrdinalIgnoreCase))
        {
            IngredientsListCalls++;

            // This is the ONLY ingredient API call IngredientService should make (cached).
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

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(response)
            });
        }

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
    }
}