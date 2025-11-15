using ShoppingList.Server.Domain.Entities;

namespace ShoppingList.Server.Web.Contracts;

public sealed record CreateFoodRequest(string Name, decimal CaloriesPer100g, decimal ProteinPer100g, decimal CarbsPer100g, decimal FatPer100g);

public sealed record FoodResponse(Guid Id, string Name, decimal CaloriesPer100g, decimal ProteinPer100g, decimal CarbsPer100g, decimal FatPer100g)
{
    public static FoodResponse FromDomain(Food food) => new(
        food.Id,
        food.Name,
        food.Per100g.Calories,
        food.Per100g.Protein,
        food.Per100g.Carbs,
        food.Per100g.Fat
    );
}
