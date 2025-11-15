using System;

namespace ShoppingList.Server.Domain.Entities;

public sealed class Food
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public Nutrition Per100g { get; private set; }
    public DateTimeOffset CreatedAt { get; }

    public Food(Guid id, string name, Nutrition per100g, DateTimeOffset? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty", nameof(name));
        if (per100g.Calories < 0 || per100g.Protein < 0 || per100g.Carbs < 0 || per100g.Fat < 0)
            throw new ArgumentOutOfRangeException(nameof(per100g), "Nutrition values cannot be negative");

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = name.Trim();
        Per100g = per100g;
        CreatedAt = createdAt ?? DateTimeOffset.UtcNow;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("Name cannot be empty", nameof(newName));
        Name = newName.Trim();
    }

    public void UpdatePer100g(Nutrition per100g)
    {
        if (per100g.Calories < 0 || per100g.Protein < 0 || per100g.Carbs < 0 || per100g.Fat < 0)
            throw new ArgumentOutOfRangeException(nameof(per100g), "Nutrition values cannot be negative");
        Per100g = per100g;
    }
}
