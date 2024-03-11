using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

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

        try
        {
            await habitRepository.AddAsync(HabitModel);
        }
        catch (SqlException e) when (e is { Number: 2627 } or { Number: 2601 })
        {
            ModelState.AddModelError("HabitModel.Name", "Name already exists.");
        }

        return RedirectToPage("./Index");
    }
}