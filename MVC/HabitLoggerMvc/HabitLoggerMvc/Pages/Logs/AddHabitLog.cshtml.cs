using HabitLoggerMvc.Helpers;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Pages.Logs;

public class AddHabitLog(IHabitLogRepository repository) : PageModel
{
    [BindProperty] public HabitLog HabitLog { get; set; } = new();

    public void OnGet(int id) => HabitLog.HabitId = id;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await repository.AddAsync(HabitLog);
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqliteException
                                                  {
                                                      SqliteExtendedErrorCode: SqliteExceptionHelper
                                                          .SQLITE_CONSTRAINT_UNIQUE
                                                  } sqliteException)
        {
            ModelState.AddModelError($"{nameof(HabitLog)}.{nameof(HabitLog.Date)}",
                sqliteException.BuildUserErrorMessage());
            return Page();
        }

        return RedirectToPage("../DetailHabit", new { id = HabitLog.HabitId });
    }
}
