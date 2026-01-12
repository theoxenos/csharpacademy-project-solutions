using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Pages;

public class UpdateHabit(IRepository<Habit> habitRepository, IHabitUnitRepository habitUnitRepository) : ErrorPageModel
{
    [BindProperty] public Habit HabitModel { get; set; }

    public List<HabitUnit> HabitUnits { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            HabitModel = await habitRepository.GetByIdAsync(id);
            IEnumerable<HabitUnit> units = await habitUnitRepository.GetAll();
            HabitUnits = units.ToList();

            return Page();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await habitRepository.UpdateAsync(HabitModel);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("HabitModel.Name",
                "An error occurred while saving. Ensure the name is unique if required.");
            IEnumerable<HabitUnit> units = await habitUnitRepository.GetAll();
            HabitUnits = units.ToList();
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
