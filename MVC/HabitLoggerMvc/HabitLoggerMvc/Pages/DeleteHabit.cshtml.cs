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
        if (id > 0)
            HabitModel = await habitRepository.GetByIdAsync(id);
        else
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await habitRepository.DeleteAsync(id);
        return RedirectToPage("./Index");
    }
}