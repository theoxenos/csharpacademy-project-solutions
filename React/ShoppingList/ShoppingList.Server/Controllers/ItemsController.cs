using Microsoft.AspNetCore.Mvc;
using ShoppingList.Server.Dtos;
using ShoppingList.Server.Models;
using ShoppingList.Server.Services;

namespace ShoppingList.Server.Controllers;

[ApiController, Route("api/[controller]")]
public class ItemsController(IItemService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ItemModel>> CreateItem([FromBody] CreateItemRequest request)
    {
        try
        {
            var item = await service.CreateItemAsync(request.ShoppingListId, request.Name.Trim(), request.Quantity);
            return CreatedAtAction(nameof(CreateItem), item);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemModel>> GetItem(int id)
    {
        var item = await service.GetOneAsync(id);

        return item != null ? Ok(item) : NotFound();
    }
    
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteItem(int id)
    {
        try
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ItemModel>> UpdateItem(int id, [FromBody] UpdateItemRequest item)
    {
        if (id != item.Id)
        {
            return BadRequest(new { error = "IDs don't match" });
        }

        try
        {
            return await service.UpdateAsync(item.Id, item.ShoppingListId, item.Name.Trim(), item.Quantity, item.IsChecked);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }
}