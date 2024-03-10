using Dapper;
using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Repositories;

public class HabitUnitRepository(HabitLoggerContext context) : IRepository<HabitUnit>
{
    public async Task<HabitUnit> AddAsync(HabitUnit model)
    {
        using var connection = await context.GetConnection();
        connection.BeginTransaction();
        await connection.ExecuteAsync("INSERT INTO HabitUnits (Name) VALUES (@Name)", model);
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<HabitUnit>> GetAll()
    {
        using var connection = await context.GetConnection();

        return await connection.QueryAsync<HabitUnit>("SELECT * FROM HabitUnits");
    }

    public async Task<HabitUnit> UpdateAsync(HabitUnit model, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<HabitUnit> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<HabitUnit> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}