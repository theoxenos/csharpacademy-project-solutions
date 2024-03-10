using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages;

public class UpdateHabit(IHabitRepository habitRepository, IRepository<HabitUnit> habitUnitRepository) : PageModel
{
    [BindProperty]
    public Habit HabitModel { get; set; }
    public List<HabitUnit> HabitUnits { get; set; }
    
    public async Task<IActionResult> OnGet(int id)
    {
        HabitModel = await habitRepository.GetHabitByIdAsync(id);
        var units = await habitUnitRepository.GetAll();
        HabitUnits = units.ToList();

        return Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        var updated = await habitRepository.UpdateAsync(HabitModel, id);
        return RedirectToPage("./Index");
    }
}