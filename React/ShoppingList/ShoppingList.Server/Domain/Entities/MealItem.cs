using System;

namespace ShoppingList.Server.Domain.Entities;

public sealed class MealItem
{
    public Guid Id { get; }
    public Guid FoodId { get; }
    public decimal QuantityGrams { get; }

    public MealItem(Guid id, Guid foodId, decimal quantityGrams)
    {
        if (quantityGrams <= 0) throw new ArgumentOutOfRangeException(nameof(quantityGrams), "Quantity must be > 0 grams");
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        FoodId = foodId;
        QuantityGrams = quantityGrams;
    }
}
