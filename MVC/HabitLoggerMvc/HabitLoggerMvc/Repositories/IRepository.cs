namespace HabitLoggerMvc.Repositories;

public interface IRepository<T>
{
    Task<T> AddAsync(T model);
    Task<IEnumerable<T>> GetAll();
    Task<T> UpdateAsync(T model);
    Task DeleteAsync(int id);
    Task<T> GetByIdAsync(int id);
}
