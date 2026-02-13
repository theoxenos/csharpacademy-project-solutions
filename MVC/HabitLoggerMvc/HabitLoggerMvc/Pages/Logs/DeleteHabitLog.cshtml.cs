using HabitLoggerMvc.Helpers;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace HabitLoggerMvc.Pages.Logs;

public class DeleteHabitLog(IHabitLogRepository repository) : PageModel
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
            HabitLog = await repository.GetByIdAsync(id.Value);
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
        try
        {
            int habitId = HabitLog.HabitId;
            await repository.DeleteAsync(HabitLog.Id);
            return RedirectToPage("../DetailHabit", new { id = habitId });
        }
        catch (SqliteException ex)
        {
            TempData["ErrorMessage"] = ex.BuildUserErrorMessage();
            return Page();
        }
    }
}
