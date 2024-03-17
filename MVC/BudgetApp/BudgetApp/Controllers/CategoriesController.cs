using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetApp.Data;

namespace BudgetApp.Controllers;

public class CategoriesController(BudgetContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await context.Categories.ToListAsync());
    }
        
    public async Task<IActionResult> List()
    {
        return PartialView("CategoriesTableRows", await context.Categories.ToListAsync());
    }
        
    public async Task<ActionResult<Category>> Detail(int id)
    {
        var category = await context.Categories.FindAsync(id);
        
        if (category == null)
        {
            return NotFound();
        }
        
        return category;
    }
        
    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] Category category)
    {
        if (id != category.Id)
        {
            return BadRequest();
        }
        
        context.Entry(category).State = EntityState.Modified;
        
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
            {
                return NotFound();
            }

            throw;
        }
        
        return RedirectToAction(nameof(List));
    }
        
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        
        return RedirectToAction(nameof(List));
    }
        
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        
        return RedirectToAction(nameof(List));
    }
        
    private bool CategoryExists(int id)
    {
        return context.Categories.Any(e => e.Id == id);
    }
}