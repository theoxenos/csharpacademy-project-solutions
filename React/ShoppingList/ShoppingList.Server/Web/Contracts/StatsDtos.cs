using ShoppingList.Server.Domain.Entities;

namespace ShoppingList.Server.Web.Contracts;

public sealed record DailySummaryResponse(decimal Calories, decimal Protein, decimal Carbs, decimal Fat)
{
    public static DailySummaryResponse FromDomain(Nutrition n)
        => new(n.Calories, n.Protein, n.Carbs, n.Fat);
}
