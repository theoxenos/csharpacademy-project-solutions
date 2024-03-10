using Dapper;
using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Repositories;

public class HabitLogRepository(HabitLoggerContext context) : IHabitLogRepository
{
    public async Task<HabitLog> AddAsync(HabitLog habitLog)
    {
        using var connection = await context.GetConnection();
        var sql = """
                    INSERT INTO HabitLogs (HabitId, Date, Quantity) VALUES (@HabitId, @Date, @Quantity);
                  SELECT * FROM HabitLogs WHERE Id = SCOPE_IDENTITY();
                  """;
        return await connection.QuerySingleAsync<HabitLog>(sql, habitLog);
    }

    public async Task<IEnumerable<HabitLog>> GetAll()
    {
        using var connection = await context.GetConnection();
        return connection.Query<HabitLog>("SELECT * FROM HabitLogs");
    }

    public async Task<HabitLog> UpdateAsync(HabitLog habitLog)
    {
        using var connection = await context.GetConnection();
        await connection.ExecuteAsync(
            "UPDATE HabitLogs SET HabitId = @HabitId, Date = @Date, Quantity = @Quantity WHERE Id = @Id",
            habitLog);

        return await connection.QuerySingleAsync<HabitLog>("SELECT * FROM HabitLogs WHERE Id = @Id", habitLog);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = await context.GetConnection();
        var result = await connection.ExecuteAsync("DELETE FROM HabitLogs WHERE Id = @Id", new { Id = id });
        if (result != 1) throw new Exception($"Something went wrong deleting habit log with Id: {id}.");
    }

    public async Task<HabitLog> GetByIdAsync(int id)
    {
        using var connection = await context.GetConnection();
        return await connection.QuerySingleAsync<HabitLog>("SELECT * FROM HabitLogs WHERE Id=@Id", new { Id = id });
    }

    public async Task<IEnumerable<HabitLog>> GetByHabitId(int id)
    {
        using var connection = await context.GetConnection();
        return await connection.QueryAsync<HabitLog>("SELECT * FROM HabitLogs WHERE HabitId = @Id", new { Id = id });
    }
}