using Dapper;
using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Repositories;

public class HabitUnitRepository(HabitLoggerContext context) : IHabitUnitRepository
{
    public async Task<HabitUnit> AddAsync(HabitUnit habitUnit)
    {
        using var connection = context.GetConnection();
        const string sql = """
                           INSERT INTO HabitUnits (Name) VALUES (@Name)
                           SELECT * FROM HabitUnits WHERE Id = SCOPE_IDENTITY();
                           """;
        return await connection.QuerySingleAsync<HabitUnit>(sql, habitUnit);
    }

    public async Task<IEnumerable<HabitUnit>> GetAll()
    {
        using var connection = context.GetConnection();

        const string sql = "SELECT * FROM HabitUnits";
        return connection.Query<HabitUnit>(sql);
    }

    public async Task<HabitUnit> UpdateAsync(HabitUnit habitUnit)
    {
        using var connection = context.GetConnection();
        const string sql = """
                           UPDATE HabitUnits SET Name = @Name WHERE Id = @Id;
                           SELECT * FROM HabitUnits WHERE Id = @Id;
                           """;

        return await connection.QuerySingleAsync<HabitUnit>(sql, habitUnit);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = context.GetConnection();
        const string sql = "DELETE FROM HabitUnits WHERE Id = @Id";
        var result = await connection.ExecuteAsync(sql, new { Id = id });
        if (result != 1) throw new Exception($"Something went wrong deleting habit unit with Id: {id}.");
    }

    public async Task<HabitUnit> GetByIdAsync(int id)
    {
        using var connection = context.GetConnection();
        const string sql = "SELECT * FROM HabitUnits WHERE Id=@Id";
        return await connection.QuerySingleAsync<HabitUnit>(sql, new { Id = id });
    }

    public async Task<bool> HabitUnitHasHabits(int id)
    {
        using var connection = context.GetConnection();
        const string sql = "SELECT COUNT(Id) FROM Habits WHERE HabitUnitId = @Id";
        var habitCount = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
        return habitCount >= 1;
    }
}