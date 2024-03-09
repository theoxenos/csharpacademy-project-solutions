using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages;

public class IndexModel(IRepository<Habit> repository) : PageModel
{
    public List<Habit> Habits { get; set; }
    
    public async Task OnGet()
    {
        var result = await repository.GetAll();
        Habits = result.ToList();
    }
}