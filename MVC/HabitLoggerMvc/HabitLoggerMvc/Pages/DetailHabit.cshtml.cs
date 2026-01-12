using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages;

public class DetailHabit(
    IRepository<Habit> habitRepository,
    IHabitUnitRepository habitUnitRepository,
    IHabitLogRepository habitLog) : PageModel
{
    [BindProperty] public Habit HabitModel { get; set; }

    [BindProperty] public HabitUnit HabitUnit { get; set; }

    [BindProperty] public List<HabitLog> HabitLogs { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            HabitModel = await habitRepository.GetByIdAsync(id);
            HabitLogs = (await habitLog.GetByHabitId(id)).OrderByDescending(l => l.Date).ToList();
            HabitUnit = await habitUnitRepository.GetByIdAsync(HabitModel.HabitUnitId);

            return Page();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        // catch (Exception ex)
        // {
        //     return RedirectToPage("/Error", new { message = ex.Message });
        // }
    }
}
