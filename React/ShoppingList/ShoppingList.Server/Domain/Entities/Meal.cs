using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Server.Domain.Entities;

public sealed class Meal
{
    public Guid Id { get; }
    public DateOnly Date { get; }
    public MealType Type { get; }
    private readonly List<MealItem> _items = new();
    public IReadOnlyList<MealItem> Items => _items;

    public Meal(Guid id, DateOnly date, MealType type, IEnumerable<MealItem>? items = null)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Date = date;
        Type = type;
        if (items != null) _items.AddRange(items);
    }

    public void AddItem(MealItem item) => _items.Add(item);

    public void AddItems(IEnumerable<MealItem> items) => _items.AddRange(items);

    public bool HasItems => _items.Count > 0;
}
