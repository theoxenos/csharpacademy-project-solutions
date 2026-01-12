using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages;

public class DeleteHabit(IRepository<Habit> habitRepository) : PageModel
{
    [BindProperty] public Habit HabitModel { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (id <= 0) return NotFound();

        try
        {
            HabitModel = await habitRepository.GetByIdAsync(id);
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

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            await habitRepository.DeleteAsync(id);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            return RedirectToPage("/Error", new { message = ex.Message });
        }
    }
}
