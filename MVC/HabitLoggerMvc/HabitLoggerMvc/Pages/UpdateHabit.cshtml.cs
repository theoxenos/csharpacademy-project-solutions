using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages;

public class UpdateHabit(IHabitRepository habitRepository, IRepository<HabitUnit> habitUnitRepository) : PageModel
{
    public Habit Habit { get; set; }
    public List<HabitUnit> HabitUnits { get; set; }
    
    public async Task<IActionResult> OnGet(int id)
    {
        Habit = await habitRepository.GetHabitByIdAsync(id);
        var units = await habitUnitRepository.GetAll();
        HabitUnits = units.ToList();

        return Page();
    }
}