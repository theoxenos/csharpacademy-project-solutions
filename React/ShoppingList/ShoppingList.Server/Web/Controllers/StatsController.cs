using Microsoft.AspNetCore.Mvc;
using ShoppingList.Server.Application.Services;
using ShoppingList.Server.Web.Contracts;

namespace ShoppingList.Server.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class StatsController(IMealService service) : ControllerBase
{
    [HttpGet("daily")]
    public async Task<ActionResult<DailySummaryResponse>> GetDaily([FromQuery] DateOnly date, CancellationToken ct)
    {
        var summary = await service.GetDailySummaryAsync(date, ct);
        return Ok(DailySummaryResponse.FromDomain(summary));
    }
}
