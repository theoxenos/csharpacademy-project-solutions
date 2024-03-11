using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages.Units;

public class Units(IRepository<HabitUnit> repository) : PageModel
{
    public List<HabitUnit> HabitUnits { get; set; } = default!;
    
    public async Task OnGetAsync()
    {
        if (HabitUnits == null)
        {
            HabitUnits = (await repository.GetAll()).ToList();
        }
    }
}