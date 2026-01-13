using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Repositories;

public interface IHabitUnitRepository : IRepository<HabitUnit>
{
    Task<bool> HabitUnitHasHabits(int id);
}
