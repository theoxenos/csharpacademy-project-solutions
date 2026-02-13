using HabitLoggerMvc.Helpers;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
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
            TempData["ErrorMessage"] = $"Habit unit with Id {id} not found";
            return Page();
        }
        catch (SqliteException ex)
        {
            TempData["ErrorMessage"] = ex.BuildUserErrorMessage();
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
            await repository.UpdateAsync(HabitUnit);
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqliteException
                                                  {
                                                      SqliteExtendedErrorCode: SqliteExceptionHelper
                                                          .SQLITE_CONSTRAINT_UNIQUE
                                                  } sqliteException)
        {
            ModelState.AddModelError("HabitUnit.Name", sqliteException.BuildUserErrorMessage());

            return Page();
        }
        catch (SqliteException exception)
        {
            TempData["ErrorMessage"] = exception.BuildUserErrorMessage();

            return Page();
        }

        return RedirectToPage("./Units");
    }
}
