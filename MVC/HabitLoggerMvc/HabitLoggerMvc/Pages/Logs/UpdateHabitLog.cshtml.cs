using HabitLoggerMvc.Helpers;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Pages.Logs;

public class UpdateHabitLog(IHabitLogRepository habitLogRepository) : PageModel
{
    [BindProperty] public HabitLog HabitLog { get; set; }
    public int HabitId => HabitLog.HabitId;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (!id.HasValue)
        {
            return RedirectToPage("../Index");
        }

        try
        {
            HabitLog = await habitLogRepository.GetByIdAsync(id.Value);
            return Page();
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Logs for habit with Id {id} not found";
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
            await habitLogRepository.UpdateAsync(HabitLog);
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

        return RedirectToPage("../DetailHabit", new { id = HabitId });
    }
}
