using Dapper;
using HabitLoggerMvc.Data;

namespace HabitLoggerMvc.Repositories;

public class Repository<T>(HabitLoggerContext context) : IRepository<T> where T : class, new()
{
    public Task<T> AddAsync(T model)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        var tableName = typeof(T).Name;
        using var connection = await context.GetConnection();
        return connection.Query<T>($"SELECT * FROM {tableName}s");
    }

    public Task<T> UpdateAsync(T model)
    {
        throw new NotImplementedException();
    }

    public Task<T> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}