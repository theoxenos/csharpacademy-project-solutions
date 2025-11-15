using Microsoft.AspNetCore.Mvc;
using ShoppingList.Server.Application.Services;
using ShoppingList.Server.Domain.Entities;
using ShoppingList.Server.Web.Contracts;

namespace ShoppingList.Server.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class MealsController(IMealService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MealResponse>>> GetByDate([FromQuery] DateOnly date, CancellationToken ct)
    {
        var meals = await service.GetByDateAsync(date, ct);
        return Ok(meals.Select(MealResponse.FromDomain).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<MealResponse>> Create([FromBody] CreateMealRequest request, CancellationToken ct)
    {
        if (request.Items is null || request.Items.Count == 0)
            return BadRequest("Meal must contain at least one item");

        if (request.Items.Any(i => i.Grams <= 0))
            return BadRequest("All items must have grams > 0");

        var items = request.Items.Select(i => (i.FoodId, i.Grams));
        try
        {
            var meal = await service.CreateAsync(request.Date, request.Type, items, ct);
            return CreatedAtAction(nameof(GetByDate), new { date = request.Date.ToString("yyyy-MM-dd") }, MealResponse.FromDomain(meal));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
