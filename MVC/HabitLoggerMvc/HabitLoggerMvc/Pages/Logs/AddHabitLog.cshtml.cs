using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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
        catch (DbUpdateException)
        {
            // Simplified check for unique constraint if needed, but for now we'll just handle it generally
            // EF Core doesn't give us the SQL Error number easily without reaching into the provider
            ModelState.AddModelError($"{nameof(HabitLog)}.{nameof(HabitLog.Date)}", "An error occurred while saving. Ensure the date is unique if required.");
            return Page();
        }

        return RedirectToPage("../DetailHabit", new { id = HabitLog.HabitId });
    }
}
