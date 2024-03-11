using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

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
        
        HabitLog = await habitLogRepository.GetByIdAsync(id.Value);

        return Page();
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
        catch (SqlException e) when(e is {Number: 2627} or {Number: 2601})
        {
            ModelState.AddModelError($"{nameof(HabitLog)}.{nameof(HabitLog.Date)}", "Date must be unique.");
            return Page();
        }

        return RedirectToPage("../DetailHabit", new { id = HabitId });
    }
}