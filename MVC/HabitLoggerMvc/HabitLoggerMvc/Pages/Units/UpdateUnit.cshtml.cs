using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages.Units;

public class UpdateUnit(IRepository<HabitUnit> repository) : PageModel
{
    [BindProperty]
    public HabitUnit HabitUnit { get; set; }
    
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if(id is null or 0) return RedirectToPage("./Units");

        HabitUnit = await repository.GetByIdAsync(id.Value);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await repository.UpdateAsync(HabitUnit);
        
        return RedirectToPage("./Units");
    }
}