using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Repositories;

public class HabitLogRepository(HabitLoggerContext context) : IHabitLogRepository
{
    public async Task<HabitLog> AddAsync(HabitLog habitLog)
    {
        context.HabitLogs.Add(habitLog);
        await context.SaveChangesAsync();
        return habitLog;
    }

    public async Task<IEnumerable<HabitLog>> GetAll() => await context.HabitLogs.ToListAsync();

    public async Task<HabitLog> UpdateAsync(HabitLog habitLog)
    {
        context.HabitLogs.Update(habitLog);
        await context.SaveChangesAsync();
        return habitLog;
    }

    public async Task DeleteAsync(int id)
    {
        HabitLog? habitLog = await context.HabitLogs.FindAsync(id);
        if (habitLog != null)
        {
            context.HabitLogs.Remove(habitLog);
            await context.SaveChangesAsync();
        }
    }

    public async Task<HabitLog> GetByIdAsync(int id) => await context.HabitLogs.FindAsync(id) ??
                                                        throw new KeyNotFoundException(
                                                            $"HabitLog with Id {id} not found.");

    public async Task<IEnumerable<HabitLog>> GetByHabitId(int id) =>
        await context.HabitLogs.Where(hl => hl.HabitId == id).ToListAsync();
}
