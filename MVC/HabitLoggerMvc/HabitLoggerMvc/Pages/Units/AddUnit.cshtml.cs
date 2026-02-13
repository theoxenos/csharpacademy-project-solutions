using HabitLoggerMvc.Helpers;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Pages.Units;

public class AddUnit(IHabitUnitRepository repository) : PageModel
{
    [BindProperty] public HabitUnit NewHabitUnit { get; set; } = new();

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await repository.AddAsync(NewHabitUnit);
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqliteException
                                                  {
                                                      SqliteExtendedErrorCode: SqliteExceptionHelper
                                                          .SQLITE_CONSTRAINT_UNIQUE
                                                  } sqliteException)
        {
            ModelState.AddModelError("NewHabitUnit.Name", sqliteException.BuildUserErrorMessage());

            return Page();
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqliteException sqliteException)
        {
            TempData["ErrorMessage"] = sqliteException.BuildUserErrorMessage();
            return Page();
        }

        return RedirectToPage("./Units");
    }
}
