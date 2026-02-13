using HabitLoggerMvc.Helpers;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace HabitLoggerMvc.Pages;

public class NewHabit(IHabitUnitRepository habitUnitRepository, IRepository<Habit> habitRepository) : PageModel
{
    [BindProperty] public Habit HabitModel { get; set; } = new();
    public List<HabitUnit> HabitUnits { get; set; } = [];

    public async Task OnGet()
    {
        try
        {
            HabitUnits = (await habitUnitRepository.GetAll()).ToList();
        }
        catch (SqliteException ex)
        {
            TempData["ErrorMessage"] = ex.BuildUserErrorMessage();
        }
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                HabitUnits = (await habitUnitRepository.GetAll()).ToList();

                return Page();
            }

            await habitRepository.AddAsync(HabitModel);
        }
        catch (SqliteException exception) when (exception.SqliteExtendedErrorCode ==
                                                SqliteExceptionHelper.SQLITE_CONSTRAINT_UNIQUE)
        {
            ModelState.AddModelError("HabitModel.Name", exception.BuildUserErrorMessage());
            HabitUnits = (await habitUnitRepository.GetAll()).ToList();

            return Page();
        }
        catch (SqliteException exception)
        {
            TempData["ErrorMessage"] = exception.BuildUserErrorMessage();
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
