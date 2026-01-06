using Dapper;
using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Repositories;

public class HabitRepository(HabitLoggerContext context) : IRepository<Habit>
{
    public async Task<Habit> AddAsync(Habit habit)
    {
        using var connection = context.GetConnection();
        const string sql = """
                           INSERT INTO Habits (Name, HabitUnitId) VALUES (@Name, @HabitUnitId);
                           SELECT * FROM Habits WHERE Id = SCOPE_IDENTITY();
                           """;
        return await connection.QuerySingleAsync<Habit>(sql, habit);
    }

    public async Task<IEnumerable<Habit>> GetAll()
    {
        using var connection = context.GetConnection();
        const string sql = "SELECT * FROM Habits";
        return connection.Query<Habit>(sql);
    }

    public async Task<Habit> UpdateAsync(Habit habit)
    {
        using var connection = context.GetConnection();
        const string sql = "UPDATE Habits SET Name = @Name, HabitUnitId = @HabitUnitId WHERE Id = @Id";
        await connection.ExecuteAsync(sql, habit);

        const string sqlQuery = "SELECT * FROM Habits WHERE Id = @Id";
        return await connection.QuerySingleAsync<Habit>(sqlQuery, habit);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = context.GetConnection();
        const string sql = "DELETE FROM Habits WHERE Id = @Id";
        var result = await connection.ExecuteAsync(sql, new { Id = id });
        if (result != 1) throw new Exception($"Something went wrong deleting habit with Id: {id}.");
    }

    public async Task<Habit> GetByIdAsync(int id)
    {
        using var connection = context.GetConnection();
        const string sql = "SELECT * FROM Habits WHERE Id=@Id";
        return await connection.QuerySingleAsync<Habit>(sql, new { Id = id });
    }
}