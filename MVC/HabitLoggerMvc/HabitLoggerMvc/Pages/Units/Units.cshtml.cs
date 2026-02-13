using HabitLoggerMvc.Helpers;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace HabitLoggerMvc.Pages.Units;

public class Units(IHabitUnitRepository repository) : PageModel
{
    public List<HabitUnit> HabitUnits { get; set; } = [];

    public async Task OnGetAsync()
    {
        try
        {
            HabitUnits = (await repository.GetAll()).ToList();
        }
        catch (SqliteException exception)
        {
            TempData["ErrorMessage"] = exception.BuildUserErrorMessage();
        }
    }
}
