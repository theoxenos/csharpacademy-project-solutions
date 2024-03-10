using Dapper;
using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Repositories;

public class HabitRepository(HabitLoggerContext context) : IRepository<Habit>
{
    public async Task<Habit> AddAsync(Habit habit)
    {
        using var connection = await context.GetConnection();
        var sql = """
                    INSERT INTO Habits (Name, HabitUnitId) VALUES (@Name, @HabitUnitId);
                  SELECT * FROM Habits WHERE Id = SCOPE_IDENTITY();
                  """;
        return await connection.QuerySingleAsync<Habit>(sql, habit);
    }

    public async Task<IEnumerable<Habit>> GetAll()
    {
        using var connection = await context.GetConnection();
        return connection.Query<Habit>($"SELECT * FROM Habits");
    }

    public async Task<Habit> UpdateAsync(Habit habit)
    {
        using var connection = await context.GetConnection();
        await connection.ExecuteAsync(
            "UPDATE Habits SET Name = @Name, HabitUnitId = @HabitUnitId WHERE Id = @Id",
            habit);

        return await connection.QuerySingleAsync<Habit>("SELECT * FROM Habits WHERE Id = @Id", habit);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = await context.GetConnection();
        var result = await connection.ExecuteAsync("DELETE FROM Habits WHERE Id = @Id", new { Id = id });
        if (result != 1) throw new Exception($"Something went wrong deleting habit with Id: {id}.");
    }

    public async Task<Habit> GetByIdAsync(int id)
    {
        using var connection = await context.GetConnection();
        return await connection.QuerySingleAsync<Habit>("SELECT * FROM Habits WHERE Id=@Id", new { Id = id });
    }
}