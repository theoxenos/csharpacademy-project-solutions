using Dapper;
using ShoppingList.Server.Abstractions;
using ShoppingList.Server.Domain.Entities;
using ShoppingList.Server.Domain.Repositories;

namespace ShoppingList.Server.Infrastructure.Repositories;

public sealed class DapperFoodRepository(IDbConnectionFactory connectionFactory) : IFoodRepository
{
    public async Task<Food> AddAsync(Food food, CancellationToken ct = default)
    {
        const string sql = @"INSERT INTO Foods (Id, Name, CaloriesPer100g, ProteinPer100g, CarbsPer100g, FatPer100g, CreatedAt)
VALUES (@Id, @Name, @Calories, @Protein, @Carbs, @Fat, @CreatedAt);";

        using var conn = connectionFactory.CreateConnection();
        await ((dynamic)conn).OpenAsync(ct);
        await conn.ExecuteAsync(sql, new
        {
            Id = food.Id,
            Name = food.Name,
            Calories = food.Per100g.Calories,
            Protein = food.Per100g.Protein,
            Carbs = food.Per100g.Carbs,
            Fat = food.Per100g.Fat,
            CreatedAt = food.CreatedAt.ToString("O")
        });

        return food;
    }

    public async Task<IReadOnlyList<Food>> GetAllAsync(CancellationToken ct = default)
    {
        const string sql = @"SELECT Id, Name, CaloriesPer100g, ProteinPer100g, CarbsPer100g, FatPer100g, CreatedAt FROM Foods ORDER BY Name;";
        using var conn = connectionFactory.CreateConnection();
        await ((dynamic)conn).OpenAsync(ct);
        var rows = await conn.QueryAsync(sql);
        return rows.Select(r => new Food(Guid.Parse((string)r.Id), (string)r.Name, new Nutrition(Convert.ToDecimal(r.CaloriesPer100g), Convert.ToDecimal(r.ProteinPer100g), Convert.ToDecimal(r.CarbsPer100g), Convert.ToDecimal(r.FatPer100g)), DateTimeOffset.Parse((string)r.CreatedAt))).ToList();
    }

    public async Task<Food?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        const string sql = @"SELECT Id, Name, CaloriesPer100g, ProteinPer100g, CarbsPer100g, FatPer100g, CreatedAt FROM Foods WHERE Id = @Id LIMIT 1;";
        using var conn = connectionFactory.CreateConnection();
        await ((dynamic)conn).OpenAsync(ct);
        var row = await conn.QueryFirstOrDefaultAsync(sql, new { Id = id.ToString().ToUpperInvariant() });
        if (row == null) return null;
        return new Food(
            Guid.Parse((string)row.Id),
            (string)row.Name,
            new Nutrition(Convert.ToDecimal(row.CaloriesPer100g), Convert.ToDecimal(row.ProteinPer100g), Convert.ToDecimal(row.CarbsPer100g), Convert.ToDecimal(row.FatPer100g)),
            DateTimeOffset.Parse((string)row.CreatedAt)
        );
    }
}
