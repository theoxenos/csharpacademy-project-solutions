using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages;

public class NewHabit(IHabitUnitRepository habitUnitRepository, IRepository<Habit> habitRepository) : PageModel
{
    [BindProperty] public Habit HabitModel { get; set; } = new();
    public List<HabitUnit> HabitUnits { get; set; }
    
    public async Task OnGet()
    {
        HabitUnits = (await habitUnitRepository.GetAll()).ToList();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            HabitUnits = (await habitUnitRepository.GetAll()).ToList();

            return Page();
        }

        await habitRepository.AddAsync(HabitModel);
        
        return RedirectToPage("./Index");
    }
}