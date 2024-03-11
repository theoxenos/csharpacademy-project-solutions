using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages.Units;

public class DeleteUnit(IHabitUnitRepository repository) : PageModel
{
    [BindProperty]
    public HabitUnit HabitUnit { get; set; }

    public bool CanDelete { get; set; }
    
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (!id.HasValue)
        {
            return RedirectToPage("./Units");
        }

        CanDelete = !await repository.HabitUnitHasHabits(id.Value);
        HabitUnit = await repository.GetByIdAsync(id.Value);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await repository.DeleteAsync(HabitUnit.Id);

        return RedirectToPage("./Units");
    }
}