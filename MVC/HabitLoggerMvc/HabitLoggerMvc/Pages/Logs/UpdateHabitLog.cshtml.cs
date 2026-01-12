using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Pages.Logs;

public class UpdateHabitLog(IHabitLogRepository habitLogRepository) : PageModel
{
    [BindProperty] public HabitLog HabitLog { get; set; }
    public int HabitId => HabitLog.HabitId;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (!id.HasValue) return RedirectToPage("../Index");

        try
        {
            HabitLog = await habitLogRepository.GetByIdAsync(id.Value);
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
        if (!ModelState.IsValid) return Page();

        try
        {
            await habitLogRepository.UpdateAsync(HabitLog);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError($"{nameof(HabitLog)}.{nameof(HabitLog.Date)}", "An error occurred while saving. Ensure the date is unique if required.");
            return Page();
        }

        return RedirectToPage("../DetailHabit", new { id = HabitId });
    }
}
