using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Repositories;

public class HabitRepository(HabitLoggerContext context) : IRepository<Habit>
{
    public async Task<Habit> AddAsync(Habit habit)
    {
        context.Habits.Add(habit);
        await context.SaveChangesAsync();
        return habit;
    }

    public async Task<IEnumerable<Habit>> GetAll()
    {
        return await context.Habits.ToListAsync();
    }

    public async Task<Habit> UpdateAsync(Habit habit)
    {
        context.Habits.Update(habit);
        await context.SaveChangesAsync();
        return habit;
    }

    public async Task DeleteAsync(int id)
    {
        var habit = await context.Habits.FindAsync(id);
        if (habit != null)
        {
            context.Habits.Remove(habit);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Habit> GetByIdAsync(int id)
    {
        return await context.Habits.FindAsync(id) ?? throw new KeyNotFoundException($"Habit with Id {id} not found.");
    }
}
