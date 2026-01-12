using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Repositories;

public class HabitUnitRepository(HabitLoggerContext context) : IHabitUnitRepository
{
    public async Task<HabitUnit> AddAsync(HabitUnit habitUnit)
    {
        context.HabitUnits.Add(habitUnit);
        await context.SaveChangesAsync();
        return habitUnit;
    }

    public async Task<IEnumerable<HabitUnit>> GetAll() => await context.HabitUnits.ToListAsync();

    public async Task<HabitUnit> UpdateAsync(HabitUnit habitUnit)
    {
        context.HabitUnits.Update(habitUnit);
        await context.SaveChangesAsync();
        return habitUnit;
    }

    public async Task DeleteAsync(int id)
    {
        HabitUnit? habitUnit = await context.HabitUnits.FindAsync(id);
        if (habitUnit != null)
        {
            context.HabitUnits.Remove(habitUnit);
            await context.SaveChangesAsync();
        }
    }

    public async Task<HabitUnit> GetByIdAsync(int id) => await context.HabitUnits.FindAsync(id) ??
                                                         throw new KeyNotFoundException(
                                                             $"HabitUnit with Id {id} not found.");

    public async Task<bool> HabitUnitHasHabits(int id) => await context.Habits.AnyAsync(h => h.HabitUnitId == id);
}
