using HabitLoggerMvc.Helpers;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace HabitLoggerMvc.Pages;

public class IndexModel(IRepository<Habit> repository) : PageModel
{
    public List<Habit> Habits { get; set; } = [];

    public async Task OnGet()
    {
        try
        {
            IEnumerable<Habit> result = await repository.GetAll();
            Habits = result.ToList();
        }
        catch (SqliteException exception)
        {
            TempData["ErrorMessage"] = exception.BuildUserErrorMessage();
        }
    }
}
