using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Pages.Units;

public class AddUnit(IHabitUnitRepository repository) : PageModel
{
    [BindProperty] public HabitUnit NewHabitUnit { get; set; } = new();

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await repository.AddAsync(NewHabitUnit);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("NewHabitUnit.Name", $"{NewHabitUnit.Name} already exists.");

            return Page();
        }

        return RedirectToPage("./Units");
    }
}
