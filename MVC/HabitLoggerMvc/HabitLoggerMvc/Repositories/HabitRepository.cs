using Dapper;
using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Repositories;

public class HabitRepository(HabitLoggerContext context) : IRepository<Habit>
{
    public async Task<Habit> GetHabitByIdAsync(int id)
    {
        var habits = await GetAll();
        return habits.FirstOrDefault(h => h.Id == id);
    }

    public async Task<Habit> AddAsync(Habit habit)
    {
        using var connection = await context.GetConnection();

        // var sql = "INSERT INTO Habits (Name, HabitUnitId)";
        // var t = "SELECT * FROM Habits WHERE Name = @Name";

        var sql = """
                    INSERT INTO Habits (Name, HabitUnitId) VALUES (@Name, @HabitUnitId);
                  SELECT * FROM Habits WHERE Id = SCOPE_IDENTITY();
                  """;
        return await connection.QueryFirstAsync<Habit>(sql, habit);

    }

    public async Task<IEnumerable<Habit>> GetAll()
    {
        using var connection = await context.GetConnection();
        return connection.Query<Habit>($"SELECT * FROM Habits");
    }

    public async Task<Habit> UpdateAsync(Habit habit, int id)
    {
        using var connection = await context.GetConnection();
        await connection.ExecuteAsync($"UPDATE Habits SET Name = @Name, HabitUnitId = @HabitUnitId WHERE Id = @Id", habit);
        
        return await connection.QueryFirstAsync<Habit>($"SELECT * FROM Habits WHERE Id = {id}");
    }

    public Task<Habit> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}