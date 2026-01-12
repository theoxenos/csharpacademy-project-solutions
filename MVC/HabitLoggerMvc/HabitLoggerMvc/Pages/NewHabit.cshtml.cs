using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Pages;

public class NewHabit(IHabitUnitRepository habitUnitRepository, IRepository<Habit> habitRepository) : PageModel
{
    [BindProperty] public Habit HabitModel { get; set; } = new();
    public List<HabitUnit> HabitUnits { get; set; }

    public async Task OnGet()
    {
        try
        {
            HabitUnits = (await habitUnitRepository.GetAll()).ToList();
        }
        catch (Exception ex)
        {
            // If we can't get units, the page won't work correctly.
            // We might want to redirect to error or just handle it.
            HabitUnits = new List<HabitUnit>();
        }
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            HabitUnits = (await habitUnitRepository.GetAll()).ToList();

            return Page();
        }

        try
        {
            await habitRepository.AddAsync(HabitModel);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("HabitModel.Name", "An error occurred while saving. Ensure the name is unique if required.");
            HabitUnits = (await habitUnitRepository.GetAll()).ToList();
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
