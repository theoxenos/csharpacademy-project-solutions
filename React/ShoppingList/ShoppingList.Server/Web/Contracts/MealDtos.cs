using ShoppingList.Server.Domain.Entities;

namespace ShoppingList.Server.Web.Contracts;

public sealed record CreateMealItem(Guid FoodId, decimal Grams);

public sealed record CreateMealRequest(DateOnly Date, MealType Type, IReadOnlyList<CreateMealItem> Items);

public sealed record MealItemResponse(Guid Id, Guid FoodId, decimal Grams);

public sealed record MealResponse(Guid Id, DateOnly Date, MealType Type, IReadOnlyList<MealItemResponse> Items)
{
    public static MealResponse FromDomain(Meal meal)
        => new(
            meal.Id,
            meal.Date,
            meal.Type,
            meal.Items.Select(i => new MealItemResponse(i.Id, i.FoodId, i.QuantityGrams)).ToList()
        );
}
