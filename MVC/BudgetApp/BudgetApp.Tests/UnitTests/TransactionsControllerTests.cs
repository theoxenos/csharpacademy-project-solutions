using BudgetApp.Controllers;
using BudgetApp.Data;
using BudgetApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Tests.UnitTests;

public class TransactionsControllerTests
{
    private BudgetContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<BudgetContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new BudgetContext(options);
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }

    [Fact]
    public async Task Index_ReturnsViewWithTransactions()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new TransactionsController(context);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<TransactionIndexViewModel>(viewResult.ViewData.Model, exactMatch: false);
        Assert.NotNull(model);
        Assert.NotEmpty(model.Transactions);
    }

    [Theory]
    [InlineData("", null, 0, 3)] // No filters
    [InlineData("Rent", null, 0, 1)] // Search term
    [InlineData("", "2024-03-01", 0, 1)] // Date filter
    [InlineData("", null, 4, 1)] // Category filter
    [InlineData("March", "2024-03-01", 1, 1)] // Multiple criteria
    [InlineData("NonExistent", null, 0, 0)] // No match
    public async Task Filter_ReturnsExpectedResults(string searchTerm, string? dateString, int categoryId,
        int expectedCount)
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new TransactionsController(context);
        DateTime? filterDate = dateString != null ? DateTime.Parse(dateString) : null;
        var indexViewModel = new TransactionIndexViewModel
        {
            TransactionFilter = searchTerm,
            DateFilter = filterDate,
            CategoryFilter = categoryId
        };

        // Act
        var result = await controller.Filter(indexViewModel);

        // Assert
        var partialViewResult = Assert.IsType<PartialViewResult>(result);
        Assert.Equal("TransactionsTableRows", partialViewResult.ViewName);
        var model = Assert.IsType<IEnumerable<Transaction>>(partialViewResult.ViewData.Model, exactMatch: false);
        var transactions = model as Transaction[] ?? model.ToArray();
        Assert.Equal(expectedCount, transactions.Length);

        if (expectedCount > 0)
        {
            Assert.All(transactions, t =>
            {
                if (!string.IsNullOrEmpty(searchTerm))
                    Assert.Contains(searchTerm, t.Comment, StringComparison.OrdinalIgnoreCase);
                if (filterDate.HasValue)
                    Assert.Equal(filterDate.Value.Date, t.Date.Date);
                if (categoryId != 0)
                    Assert.Equal(categoryId, t.CategoryId);
            });
        }
    }

    [Fact]
    public async Task Create_ValidModel_AddsTransactionAndReturnsPartialView()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new TransactionsController(context);
        var initialCount = await context.Transactions.CountAsync();
        var transaction = new Transaction
        {
            Amount = 100,
            CategoryId = 1,
            Comment = "Test Create",
            Date = DateTime.Now
        };

        // Act
        var result = await controller.Create(transaction);

        // Assert
        var partialViewResult = Assert.IsType<PartialViewResult>(result);
        Assert.Equal("TransactionsTableRows", partialViewResult.ViewName);

        var model = Assert.IsType<IEnumerable<Transaction>>(partialViewResult.ViewData.Model, exactMatch: false);
        var transactions = model.ToList();

        Assert.Equal(initialCount + 1, await context.Transactions.CountAsync());
        Assert.Contains(transactions, t => t is { Comment: "Test Create", Amount: 100 });

        var addedTransaction = await context.Transactions.FirstOrDefaultAsync(t => t.Comment == "Test Create");
        Assert.NotNull(addedTransaction);
        Assert.Equal(100, addedTransaction.Amount);
        Assert.Equal(1, addedTransaction.CategoryId);
    }

    [Fact]
    public async Task Create_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new TransactionsController(context);
        var initialCount = await context.Transactions.CountAsync();
        var transaction = new Transaction
        {
            Amount = -100,
            CategoryId = 1,
            Comment = "Test Create",
            Date = DateTime.Now
        };

        // Act
        controller.ModelState.AddModelError("Amount", "Amount must be more than 0");
        var result = await controller.Create(transaction);

        // Assert
        Assert.IsType<BadRequestResult>(result);

        Assert.Equal(initialCount, await context.Transactions.CountAsync());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(420)]
    public async Task Detail_InvalidIds_Returns_NotFound(int id)
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new TransactionsController(context);

        // Act
        var result = await controller.Detail(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Detail_ValidId_Returns_ViewWithTransaction()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new TransactionsController(context);
        var firstTransaction = await context.Transactions.FirstAsync();

        // Act
        var result = await controller.Detail(firstTransaction.Id);

        // Assert
        var viewResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<Transaction>(viewResult.Value);
        Assert.Equal(firstTransaction.Id, model.Id);
        Assert.Equal(firstTransaction.Amount, model.Amount);
        Assert.Equal(firstTransaction.Comment, model.Comment);
        Assert.Equal(firstTransaction.Date, model.Date);
        Assert.Equal(firstTransaction.CategoryId, model.CategoryId);
    }

    [Fact]
    public async Task Update_ValidModel_Returns_PartialView()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new TransactionsController(context);
        const int originalId = 1;
        var toUpdateTransaction = new Transaction
            { Id = originalId, Amount = 250, Comment = "Updated Comment", CategoryId = 2 };

        // Act
        var result = await controller.Update(originalId, toUpdateTransaction);

        // Assert
        var partialViewResult = Assert.IsType<PartialViewResult>(result);
        Assert.Equal("TransactionsTableRows", partialViewResult.ViewName);

        var model = Assert.IsType<IEnumerable<Transaction>>(partialViewResult.ViewData.Model, exactMatch: false);
        Assert.NotNull(model);

        var updatedTransaction = await context.Transactions.FirstOrDefaultAsync(t => t.Id == originalId);
        Assert.NotNull(updatedTransaction);
        Assert.Equal(250, updatedTransaction.Amount);
        Assert.Equal("Updated Comment", updatedTransaction.Comment);
        Assert.Equal(2, updatedTransaction.CategoryId);
    }
    
    [Fact]
    public async Task Delete_DeletesTransaction_Returns_PartialView_WithUpdatedList()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new TransactionsController(context);
        var transactionToDelete = await context.Transactions.FirstAsync();
        
        // Act
        var result = await controller.Delete(transactionToDelete.Id);
        
        // Assert
        var partialViewResult = Assert.IsType<PartialViewResult>(result);
        Assert.Equal("TransactionsTableRows", partialViewResult.ViewName);

        var model = Assert.IsType<IEnumerable<Transaction>>(partialViewResult.ViewData.Model, exactMatch: false);
        var transactions = model.ToList();

        Assert.DoesNotContain(transactions, t => t.Id == transactionToDelete.Id);
    }
}