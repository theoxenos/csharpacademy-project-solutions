using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Repositories;

public class HabitRepository(HabitLoggerContext context) : Repository<Habit>(context), IHabitRepository
{
    public async Task<Habit> GetHabitByIdAsync(int id)
    {
        var habits = await GetAll();
        return habits.FirstOrDefault(h => h.Id == id);
    }
}