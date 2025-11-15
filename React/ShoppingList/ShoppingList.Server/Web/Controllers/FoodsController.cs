using Microsoft.AspNetCore.Mvc;
using ShoppingList.Server.Application.Services;
using ShoppingList.Server.Domain.Entities;
using ShoppingList.Server.Web.Contracts;

namespace ShoppingList.Server.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class FoodsController(IFoodService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FoodResponse>>> GetAll(CancellationToken ct)
    {
        var foods = await service.ListAsync(ct);
        return Ok(foods.Select(FoodResponse.FromDomain).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<FoodResponse>> Create([FromBody] CreateFoodRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Name is required");
        if (request.CaloriesPer100g < 0 || request.ProteinPer100g < 0 || request.CarbsPer100g < 0 || request.FatPer100g < 0)
            return BadRequest("Nutrition values cannot be negative");

        var nutrition = new Nutrition(request.CaloriesPer100g, request.ProteinPer100g, request.CarbsPer100g, request.FatPer100g);
        var created = await service.CreateAsync(request.Name.Trim(), nutrition, ct);
        var dto = FoodResponse.FromDomain(created);
        return CreatedAtAction(nameof(GetAll), new { id = dto.Id }, dto);
    }
}
