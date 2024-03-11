using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages;

public class UpdateHabit(IRepository<Habit> habitRepository, IHabitUnitRepository habitUnitRepository) : PageModel
{
    [BindProperty]
    public Habit HabitModel { get; set; }
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
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        var updated = await habitRepository.UpdateAsync(HabitModel);
        return RedirectToPage("./Index");
    }
}