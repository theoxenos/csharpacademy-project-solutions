namespace HabitLoggerMvc.Repositories;

public interface IRepository<T> where T : class, new()
{
    Task<T> AddAsync(T model);
    Task<IEnumerable<T>> GetAll();
    Task<T> UpdateAsync(T model);
    Task<T> DeleteAsync(int id);
}