using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Pages;

public class NewHabit(IHabitUnitRepository habitUnitRepository, IRepository<Habit> habitRepository) : ErrorPageModel
{
    [BindProperty] public Habit HabitModel { get; set; } = new();
    public List<HabitUnit> HabitUnits { get; set; } = [];

    public async Task OnGet()
    {
        try
        {
            HabitUnits = (await habitUnitRepository.GetAll()).ToList();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Something went wrong: {ex.Message}";
        }
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
        catch (DbUpdateException exception)
        {
            if (exception.InnerException is not SqliteException { SqliteErrorCode: 19 })
            {
                ErrorMessage = exception.Message;
                return Page();
            }

            ModelState.AddModelError("HabitModel.Name", $"{HabitModel.Name} already exists.");
            HabitUnits = (await habitUnitRepository.GetAll()).ToList();

            return Page();
        }

        return RedirectToPage("./Index");
    }
}
