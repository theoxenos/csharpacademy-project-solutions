using HabitLoggerMvc.Helpers;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace HabitLoggerMvc.Pages.Units;

public class DeleteUnit(IHabitUnitRepository repository) : PageModel
{
    [BindProperty] public HabitUnit HabitUnit { get; set; }

    public bool CanDelete { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (!id.HasValue)
        {
            return RedirectToPage("./Units");
        }

        try
        {
            CanDelete = !await repository.HabitUnitHasHabits(id.Value);
            HabitUnit = await repository.GetByIdAsync(id.Value);
            return Page();
        }
        catch (KeyNotFoundException exception)
        {
            TempData["ErrorMessage"] = exception.Message;
            return Page();
        }
        catch (SqliteException exception)
        {
            TempData["ErrorMessage"] = exception.BuildUserErrorMessage();
            return Page();
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
            await repository.DeleteAsync(HabitUnit.Id);
            return RedirectToPage("./Units");
        }
        catch (SqliteException exception)
        {
            TempData["ErrorMessage"] = exception.BuildUserErrorMessage();
            return Page();
        }
    }
}
