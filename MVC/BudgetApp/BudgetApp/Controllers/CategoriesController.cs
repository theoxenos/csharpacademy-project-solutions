using BudgetApp.Data;
using BudgetApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Controllers;

public class CategoriesController(BudgetContext context) : Controller
{
    public async Task<IActionResult> Index() => View(await context.Categories.ToListAsync());

    public async Task<IActionResult> List() =>
        PartialView("CategoriesTableRows", await context.Categories.ToListAsync());

    public async Task<ActionResult<Category>> Detail(int id)
    {
        Category? category = await context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return category;
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, [FromBody] Category category)
    {
        if (id != category.Id || !ModelState.IsValid)
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(List));
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Category? category = await context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(List));
    }

    private bool CategoryExists(int id) => context.Categories.Any(e => e.Id == id);
}
