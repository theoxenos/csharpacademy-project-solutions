using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages.Units;

public class Units(IHabitUnitRepository repository) : PageModel
{
    public List<HabitUnit> HabitUnits { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (HabitUnits == null)
            {
                HabitUnits = (await repository.GetAll()).ToList();
            }

            return Page();
        }
        catch (Exception ex)
        {
            return RedirectToPage("/Error", new { message = ex.Message });
        }
    }
}
