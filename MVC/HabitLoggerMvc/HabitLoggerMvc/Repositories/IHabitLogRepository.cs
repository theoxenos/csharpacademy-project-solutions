using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Repositories;

public interface IHabitLogRepository : IRepository<HabitLog>
{
    Task<IEnumerable<HabitLog>> GetByHabitId(int id);
}
