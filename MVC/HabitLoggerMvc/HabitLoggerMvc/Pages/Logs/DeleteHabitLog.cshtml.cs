using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages.Logs;

public class DeleteHabitLog(IHabitLogRepository repository) : PageModel
{
    [BindProperty] public HabitLog HabitLog { get; set; }
    public int HabitId => HabitLog.HabitId;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (!id.HasValue) return RedirectToPage("../Index");

        try
        {
            HabitLog = await repository.GetByIdAsync(id.Value);
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
        try
        {
            await repository.DeleteAsync(HabitLog.Id);
            return RedirectToPage("../DetailHabit", new { id = HabitId });
        }
        catch (Exception ex)
        {
            return RedirectToPage("/Error", new { message = ex.Message });
        }
    }
}
