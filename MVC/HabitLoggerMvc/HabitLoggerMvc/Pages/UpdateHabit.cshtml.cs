using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

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
        catch (SqlException e) when (e is { Number: 2627 } or { Number: 2601 })
        {
            ModelState.AddModelError("HabitModel.Name", "Name already exists.");
            return Page();
        }

        return RedirectToPage("./Index");
    }
}