using HabitLoggerMvc.Helpers;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace HabitLoggerMvc.Pages;

public class DeleteHabit(IRepository<Habit> habitRepository) : PageModel
{
    [BindProperty] public Habit HabitModel { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        try
        {
            HabitModel = await habitRepository.GetByIdAsync(id);
            return Page();
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Habit with Id {id} not found";
            return Page();
        }
        catch (SqliteException exception)
        {
            TempData["ErrorMessage"] = exception.BuildUserErrorMessage();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            await habitRepository.DeleteAsync(id);
            return RedirectToPage("./Index");
        }
        catch (SqliteException exception)
        {
            TempData["ErrorMessage"] = exception.BuildUserErrorMessage();
            return Page();
        }
    }
}
