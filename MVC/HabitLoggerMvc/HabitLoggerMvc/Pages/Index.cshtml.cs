using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages;

public class IndexModel(IRepository<Habit> repository) : PageModel
{
    public List<Habit> Habits { get; set; }

    public async Task<IActionResult> OnGet()
    {
        try
        {
            IEnumerable<Habit> result = await repository.GetAll();
            Habits = result.ToList();
            return Page();
        }
        catch (Exception ex)
        {
            return RedirectToPage("/Error", new { message = ex.Message });
        }
    }
}
