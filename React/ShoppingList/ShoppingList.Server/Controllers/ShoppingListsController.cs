using Microsoft.AspNetCore.Mvc;
using ShoppingList.Server.Dtos;
using ShoppingList.Server.Models;
using ShoppingList.Server.Services;

namespace ShoppingList.Server.Controllers;

[ApiController, Route("api/[controller]")]
public class ShoppingListsController(IShoppingListService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ShoppingListModel>> CreateShoppingList([FromBody] CreateShoppingListRequest request)
    {
        if (string.IsNullOrEmpty(request.Name.Trim()))
        {
            return BadRequest("Name is required");
        }

        return CreatedAtAction(nameof(CreateShoppingList), await service.CreateShoppingListAsync(request.Name.Trim()));
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ShoppingListModel>>> GetAllShoppingLists()
    {
        return Ok(await service.GetAllShoppingListsAsync());
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ShoppingListModel?>> GetShoppingList(int id)
    {
        return Ok(await service.GetShoppingListAsync(id));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ShoppingListModel?>> UpdateShoppingList(int id,
        [FromBody] UpdateShoppingListRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(new { error = "IDs don't match" });
        }
        
        var updatedList = await service.UpdateShoppingListAsync(id, request.Name.Trim());
        return updatedList != null ? Ok(updatedList) : NotFound();
    }
}