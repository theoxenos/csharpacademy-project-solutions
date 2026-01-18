using System.Net;
using System.Net.Http.Json;
using BudgetApp.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetApp.Tests.IntegrationTests;

public class TransactionsIntegrationTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Create_InvalidModel_ReturnsBadRequest_Integration()
    {
        // Arrange
        var client = factory.CreateClient();
        var invalidTransaction = new
        {
            Amount = -100, // Invalid: must be > 0
            CategoryId = 0, // Invalid: must be > 0
            Comment = "Sh", // Invalid: min length 3
            Date = DateTime.Now
        };

        // Act
        var response = await client.PostAsJsonAsync("/Transactions/Create", invalidTransaction);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        // Verify it was NOT added to the database
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BudgetContext>();
        var count = await context.Transactions.CountAsync(t => t.Comment == "Sh");
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task Create_InvalidModel_NoAmount_ReturnsBadRequest_Integration()
    {
        // Arrange
        var client = factory.CreateClient();
        var invalidTransaction = new
        {
            // Amount missing or 0
            CategoryId = 1,
            Comment = "No Amount",
            Date = DateTime.Now
        };

        // Act
        var response = await client.PostAsJsonAsync("/Transactions/Create", invalidTransaction);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        // Verify it was NOT added to the database
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BudgetContext>();
        var count = await context.Transactions.CountAsync(t => t.Comment == "No Amount");
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task Create_ValidModel_Returns400_WhenAntiforgeryMissing()
    {
        // Arrange
        var client = factory.CreateClient();

        var validTransaction = new
        {
            Amount = 100,
            CategoryId = 1,
            Comment = "Antiforgery Test",
            Date = DateTime.Now
        };

        // Act
        var response = await client.PostAsJsonAsync("/Transactions/Create", validTransaction);

        // Assert
        // Now that app.UseAntiforgery() is added, this SHOULD return 400 Bad Request because of missing token.
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        // Verify it was NOT added to the database
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BudgetContext>();
        var exists = await context.Transactions.AnyAsync(t => t.Comment == "Antiforgery Test");
        Assert.False(exists,
            "Transaction should NOT have been added because Antiforgery validation should have failed.");
    }
}