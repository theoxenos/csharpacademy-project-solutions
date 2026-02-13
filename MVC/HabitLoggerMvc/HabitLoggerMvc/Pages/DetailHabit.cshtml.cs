using HabitLoggerMvc.Helpers;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace HabitLoggerMvc.Pages;

public class DetailHabit(
    IRepository<Habit> habitRepository,
    IHabitUnitRepository habitUnitRepository,
    IHabitLogRepository habitLog) : PageModel
{
    [BindProperty] public Habit HabitModel { get; set; }

    [BindProperty] public HabitUnit HabitUnit { get; set; }

    [BindProperty] public List<HabitLog> HabitLogs { get; set; }

    public async Task OnGetAsync(int id)
    {
        try
        {
            HabitModel = await habitRepository.GetByIdAsync(id);
            HabitLogs = (await habitLog.GetByHabitId(id)).OrderByDescending(l => l.Date).ToList();
            HabitUnit = await habitUnitRepository.GetByIdAsync(HabitModel.HabitUnitId);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Logs for habit with Id {id} not found";
        }
        catch (SqliteException exception)
        {
            TempData["ErrorMessage"] = exception.BuildUserErrorMessage();
        }
    }
}
