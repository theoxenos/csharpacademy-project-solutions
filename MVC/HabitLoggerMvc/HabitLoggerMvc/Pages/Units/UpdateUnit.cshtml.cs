using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Pages.Units;

public class UpdateUnit(IHabitUnitRepository repository) : PageModel
{
    [BindProperty] public HabitUnit HabitUnit { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null or 0)
        {
            return RedirectToPage("./Units");
        }

        try
        {
            HabitUnit = await repository.GetByIdAsync(id.Value);
            return Page();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return RedirectToPage("/Error", new { message = ex.Message });
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await repository.UpdateAsync(HabitUnit);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("HabitUnit.Name",
                "An error occurred while saving. Ensure the name is unique if required.");

            return Page();
        }

        return RedirectToPage("./Units");
    }
}
