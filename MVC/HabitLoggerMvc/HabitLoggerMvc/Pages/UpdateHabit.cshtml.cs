using HabitLoggerMvc.Helpers;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Pages;

public class UpdateHabit(IRepository<Habit> habitRepository, IHabitUnitRepository habitUnitRepository) : PageModel
{
    [BindProperty] public Habit HabitModel { get; set; }

    public List<HabitUnit> HabitUnits { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            HabitModel = await habitRepository.GetByIdAsync(id);
            IEnumerable<HabitUnit> units = await habitUnitRepository.GetAll();
            HabitUnits = units.ToList();

            return Page();
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Habit with Id {id} not found";
            return Page();
        }
        catch (SqliteException ex)
        {
            TempData["ErrorMessage"] = ex.BuildUserErrorMessage();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await habitRepository.UpdateAsync(HabitModel);
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqliteException
                                                  {
                                                      SqliteExtendedErrorCode: SqliteExceptionHelper
                                                          .SQLITE_CONSTRAINT_UNIQUE
                                                  } sqliteException)
        {
            ModelState.AddModelError("HabitModel.Name", sqliteException.BuildUserErrorMessage());
            IEnumerable<HabitUnit> units = await habitUnitRepository.GetAll();
            HabitUnits = units.ToList();
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
