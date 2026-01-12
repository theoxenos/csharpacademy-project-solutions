using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Pages;

public class UpdateHabit(IRepository<Habit> habitRepository, IHabitUnitRepository habitUnitRepository) : PageModel
{
    [BindProperty] public Habit HabitModel { get; set; }

    public List<HabitUnit> HabitUnits { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        HabitModel = await habitRepository.GetByIdAsync(id);
        var units = await habitUnitRepository.GetAll();
        HabitUnits = units.ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await habitRepository.UpdateAsync(HabitModel);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("HabitModel.Name", "An error occurred while saving. Ensure the name is unique if required.");
            var units = await habitUnitRepository.GetAll();
            HabitUnits = units.ToList();
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
