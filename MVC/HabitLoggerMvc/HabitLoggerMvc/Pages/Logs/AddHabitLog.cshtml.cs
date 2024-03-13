using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace HabitLoggerMvc.Pages.Logs;

public class AddHabitLog(IHabitLogRepository repository) : PageModel
{
    [BindProperty] public HabitLog HabitLog { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        HabitLog.HabitId = id;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await repository.AddAsync(HabitLog);
        }
        catch (SqlException e) when (e is { Number: 2627 } or { Number: 2601 })
        {
            ModelState.AddModelError($"{nameof(HabitLog)}.{nameof(HabitLog.Date)}", "Date must be unique.");
            return Page();
        }

        return RedirectToPage("../DetailHabit", new { id = HabitLog.HabitId });
    }
}