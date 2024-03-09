using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Repositories;

public interface IHabitRepository : IRepository<Habit>
{
    Task<Habit> GetHabitByIdAsync(int id);
}