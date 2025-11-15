using Dapper;
using ShoppingList.Server.Abstractions;
using ShoppingList.Server.Domain.Entities;
using ShoppingList.Server.Domain.Repositories;

namespace ShoppingList.Server.Infrastructure.Repositories;

public sealed class DapperMealRepository(IDbConnectionFactory connectionFactory) : IMealRepository
{
    public async Task<Meal> AddAsync(Meal meal, CancellationToken ct = default)
    {
        using var conn = connectionFactory.CreateConnection();
        await ((dynamic)conn).OpenAsync(ct);
        using var tx = conn.BeginTransaction();

        const string insertMeal = @"INSERT INTO Meals (Id, Date, Type) VALUES (@Id, @Date, @Type);";
        await conn.ExecuteAsync(insertMeal, new
        {
            Id = meal.Id,
            Date = meal.Date.ToString("yyyy-MM-dd"),
            Type = (int)meal.Type
        }, tx);

        const string insertItem = @"INSERT INTO MealItems (Id, MealId, FoodId, QuantityGrams) VALUES (@Id, @MealId, @FoodId, @QuantityGrams);";
        foreach (var item in meal.Items)
        {
            await conn.ExecuteAsync(insertItem, new
            {
                Id = item.Id,
                MealId = meal.Id,
                FoodId = item.FoodId,
                QuantityGrams = item.QuantityGrams
            }, tx);
        }

        tx.Commit();
        return meal;
    }

    public async Task<IReadOnlyList<Meal>> GetByDateAsync(DateOnly date, CancellationToken ct = default)
    {
        using var conn = connectionFactory.CreateConnection();
        await ((dynamic)conn).OpenAsync(ct);

        const string mealsSql = @"SELECT Id, Date, Type FROM Meals WHERE Date = @Date ORDER BY Type;";
        var mealRows = await conn.QueryAsync(mealsSql, new { Date = date.ToString("yyyy-MM-dd") });
        var meals = mealRows
            .Select(r => new Meal(Guid.Parse((string)r.Id), DateOnly.Parse((string)r.Date), (MealType)(int)r.Type))
            .ToDictionary(m => m.Id);

        if (meals.Count == 0) return Array.Empty<Meal>();

        const string itemsSql = @"SELECT Id, MealId, FoodId, QuantityGrams FROM MealItems WHERE MealId IN @Ids;";
        var itemRows = await conn.QueryAsync(itemsSql, new { Ids = meals.Keys.Select(id => id.ToString()).ToArray() });
        foreach (var r in itemRows)
        {
            var item = new MealItem(Guid.Parse((string)r.Id), Guid.Parse((string)r.FoodId), Convert.ToDecimal(r.QuantityGrams));
            var mealId = Guid.Parse((string)r.MealId);
            if (meals.TryGetValue(mealId, out var meal))
            {
                meal.AddItem(item);
            }
        }

        return meals.Values.OrderBy(m => m.Type).ToList();
    }

    public async Task<Nutrition> GetDailySummaryAsync(DateOnly date, CancellationToken ct = default)
    {
        using var conn = connectionFactory.CreateConnection();
        await ((dynamic)conn).OpenAsync(ct);

        const string sql = @"
SELECT 
    COALESCE(SUM((mi.QuantityGrams/100.0) * f.CaloriesPer100g), 0) AS Calories,
    COALESCE(SUM((mi.QuantityGrams/100.0) * f.ProteinPer100g), 0) AS Protein,
    COALESCE(SUM((mi.QuantityGrams/100.0) * f.CarbsPer100g), 0) AS Carbs,
    COALESCE(SUM((mi.QuantityGrams/100.0) * f.FatPer100g), 0) AS Fat
FROM MealItems mi
JOIN Meals m ON m.Id = mi.MealId
JOIN Foods f ON f.Id = mi.FoodId
WHERE m.Date = @Date;";

        var row = await conn.QueryFirstOrDefaultAsync(sql, new { Date = date.ToString("yyyy-MM-dd") });
        if (row == null) return Nutrition.Zero;
        return new Nutrition(
            Convert.ToDecimal(row.Calories),
            Convert.ToDecimal(row.Protein),
            Convert.ToDecimal(row.Carbs),
            Convert.ToDecimal(row.Fat)
        );
    }
}
