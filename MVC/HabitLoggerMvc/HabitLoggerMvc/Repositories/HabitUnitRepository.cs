using Dapper;
using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Repositories;

public class HabitUnitRepository(HabitLoggerContext context) : IHabitUnitRepository
{
    public async Task<HabitUnit> AddAsync(HabitUnit habitUnit)
    {
        using var connection = await context.GetConnection();
        var sql = """
                  INSERT INTO HabitUnits (Name) VALUES (@Name)
                  SELECT * FROM HabitUnits WHERE Id = SCOPE_IDENTITY();
                  """;
        return await connection.QuerySingleAsync<HabitUnit>(sql, habitUnit);
    }

    public async Task<IEnumerable<HabitUnit>> GetAll()
    {
        using var connection = await context.GetConnection();

        return await connection.QueryAsync<HabitUnit>("SELECT * FROM HabitUnits");
    }

    public async Task<HabitUnit> UpdateAsync(HabitUnit habitUnit)
    {
       using var connection = await context.GetConnection();
       var sql = """
                 UPDATE HabitUnits SET Name = @Name WHERE Id = @Id;
                 SELECT * FROM HabitUnits WHERE Id = @Id;
                 """;

        return await connection.QuerySingleAsync<HabitUnit>(sql, habitUnit);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = await context.GetConnection();
        var result = await connection.ExecuteAsync("DELETE FROM HabitUnits WHERE Id = @Id", new { Id = id });
        if (result != 1) throw new Exception($"Something went wrong deleting habit unit with Id: {id}.");
    }

    public async Task<HabitUnit> GetByIdAsync(int id)
    {
        using var connection = await context.GetConnection();
        return await connection.QuerySingleAsync<HabitUnit>("SELECT * FROM HabitUnits WHERE Id=@Id", new { Id = id });
    }

    public async Task<bool> HabitUnitHasHabits(int id)
    {
        using var connection = await context.GetConnection();
        return await connection.ExecuteScalarAsync<int>("SELECT COUNT(Id) FROM Habits WHERE HabitUnitId = @Id",
            new { Id = id }) >= 1;
    }
}