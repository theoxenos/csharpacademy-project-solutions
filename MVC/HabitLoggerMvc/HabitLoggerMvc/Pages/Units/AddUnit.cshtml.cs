using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace HabitLoggerMvc.Pages.Units;

public class AddUnit(IHabitUnitRepository repository) : PageModel
{
    [BindProperty]
    public HabitUnit NewHabitUnit { get; set; }
    
    public IActionResult OnGet()
    {
        return Page();
    }

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
        catch (SqlException e) when (e is { Number: 2627 } or { Number: 2601 })
        {
            ModelState.AddModelError("NewHabitUnit.Name", "Name already exists.");

            return Page();
        }

        return RedirectToPage("./Units");
    }
}