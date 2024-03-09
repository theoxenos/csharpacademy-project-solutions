namespace HabitLoggerMvc.Repositories;

public abstract class HabitRepository<Habit> : IRepository<Habit> where Habit : class, new()
{
    public Task<Habit> AddAsync(Habit model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Habit>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Habit> UpdateAsync(Habit model)
    {
        throw new NotImplementedException();
    }

    public Task<Habit> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}