using Dapper;
using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Repositories;

public class HabitLogRepository(HabitLoggerContext context) : IHabitLogRepository
{
    public async Task<HabitLog> AddAsync(HabitLog habitLog)
    {
        using var connection = context.GetConnection();
        const string sql = """
                             INSERT INTO HabitLogs (HabitId, Date, Quantity) VALUES (@HabitId, @Date, @Quantity);
                           SELECT * FROM HabitLogs WHERE Id = SCOPE_IDENTITY();
                           """;
        return await connection.QuerySingleAsync<HabitLog>(sql, habitLog);
    }

    public async Task<IEnumerable<HabitLog>> GetAll()
    {
        using var connection = context.GetConnection();
        const string sql = "SELECT * FROM HabitLogs";
        return connection.Query<HabitLog>(sql);
    }

    public async Task<HabitLog> UpdateAsync(HabitLog habitLog)
    {
        using var connection = context.GetConnection();
        const string sql = "UPDATE HabitLogs SET HabitId = @HabitId, Date = @Date, Quantity = @Quantity WHERE Id = @Id";
        await connection.ExecuteAsync(sql, habitLog);

        const string sqlQuery = "SELECT * FROM HabitLogs WHERE Id = @Id";
        return await connection.QuerySingleAsync<HabitLog>(sqlQuery, habitLog);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = context.GetConnection();
        const string sql = "DELETE FROM HabitLogs WHERE Id = @Id";
        var result = await connection.ExecuteAsync(sql, new { Id = id });
        if (result != 1) throw new Exception($"Something went wrong deleting habit log with Id: {id}.");
    }

    public async Task<HabitLog> GetByIdAsync(int id)
    {
        using var connection = context.GetConnection();
        const string sql = "SELECT * FROM HabitLogs WHERE Id=@Id";
        return await connection.QuerySingleAsync<HabitLog>(sql, new { Id = id });
    }

    public async Task<IEnumerable<HabitLog>> GetByHabitId(int id)
    {
        using var connection = context.GetConnection();
        const string sql = "SELECT * FROM HabitLogs WHERE HabitId = @Id";
        return await connection.QueryAsync<HabitLog>(sql, new { Id = id });
    }
}