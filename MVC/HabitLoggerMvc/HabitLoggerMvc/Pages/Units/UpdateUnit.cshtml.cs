using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace HabitLoggerMvc.Pages.Units;

public class UpdateUnit(IHabitUnitRepository repository) : PageModel
{
    [BindProperty] public HabitUnit HabitUnit { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null or 0) return RedirectToPage("./Units");

        HabitUnit = await repository.GetByIdAsync(id.Value);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await repository.UpdateAsync(HabitUnit);
        }
        catch (SqlException e) when (e is { Number: 2627 } or { Number: 2601 })
        {
            ModelState.AddModelError("HabitUnit.Name", "Name already exists.");

            return Page();
        }

        return RedirectToPage("./Units");
    }
}