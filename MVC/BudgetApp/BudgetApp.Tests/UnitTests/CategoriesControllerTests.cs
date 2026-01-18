using BudgetApp.Controllers;
using BudgetApp.Data;
using BudgetApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Tests.UnitTests;

public class CategoriesControllerTests
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
    public async Task Index_ReturnsViewWithCategories()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new CategoriesController(context);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<IEnumerable<Category>>(viewResult.ViewData.Model, exactMatch: false);
        Assert.NotEmpty(model);
    }

    [Fact]
    public async Task List_ReturnsPartialViewWithCategories()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new CategoriesController(context);

        // Act
        var result = await controller.List();

        // Assert
        var partialViewResult = Assert.IsType<PartialViewResult>(result);
        Assert.Equal("CategoriesTableRows", partialViewResult.ViewName);
        var model = Assert.IsType<IEnumerable<Category>>(partialViewResult.ViewData.Model, exactMatch: false);
        Assert.NotEmpty(model);
    }

    [Fact]
    public async Task Detail_ReturnsCategory_WhenIdIsValid()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new CategoriesController(context);
        var expectedCategory = context.Categories.First();

        // Act
        var result = await controller.Detail(expectedCategory.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Category>>(result);
        var category = Assert.IsType<Category>(actionResult.Value);
        Assert.Equal(expectedCategory.Id, category.Id);
    }

    [Fact]
    public async Task Detail_ReturnsNotFound_WhenIdIsInvalid()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new CategoriesController(context);

        // Act
        var result = await controller.Detail(999);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Category>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task Create_RedirectsToList_WhenModelIsValid()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new CategoriesController(context);
        var newCategory = new Category { Name = "New Category", Color = "#123456" };

        // Act
        var result = await controller.Create(newCategory);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("List", redirectResult.ActionName);
        Assert.True(context.Categories.Any(c => c.Name == "New Category"));
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelIsInvalid()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new CategoriesController(context);
        controller.ModelState.AddModelError("Name", "Required");
        var newCategory = new Category { Color = "#123456" };

        // Act
        var result = await controller.Create(newCategory);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Update_RedirectsToList_WhenModelIsValid()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new CategoriesController(context);
        var category = context.Categories.First();
        
        // Detach the tracked entity to simulate it being sent from a client
        context.Entry(category).State = EntityState.Detached;
        
        var updatedCategory = new Category { Id = category.Id, Name = "Updated", Color = "#654321" };

        // Act
        var result = await controller.Update(category.Id, updatedCategory);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("List", redirectResult.ActionName);
        
        Assert.Equal("Updated", context.Categories.Find(category.Id)!.Name);
    }

    [Fact]
    public async Task Delete_RedirectsToList_WhenIdIsValid()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new CategoriesController(context);
        var category = context.Categories.First();

        // Act
        var result = await controller.Delete(category.Id);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("List", redirectResult.ActionName);
        Assert.Null(context.Categories.Find(category.Id));
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new CategoriesController(context);
        var category = context.Categories.First();
        var updatedCategory = new Category { Id = category.Id + 1, Name = "Updated" };

        // Act
        var result = await controller.Update(category.Id, updatedCategory);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new CategoriesController(context);
        var updatedCategory = new Category { Id = 999, Name = "Updated" };

        // Act
        var result = await controller.Update(999, updatedCategory);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenIdIsInvalid()
    {
        // Arrange
        await using var context = GetDbContext();
        var controller = new CategoriesController(context);

        // Act
        var result = await controller.Delete(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}