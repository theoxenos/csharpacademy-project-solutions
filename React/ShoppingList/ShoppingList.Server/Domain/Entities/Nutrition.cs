namespace ShoppingList.Server.Domain.Entities;

public readonly record struct Nutrition(decimal Calories, decimal Protein, decimal Carbs, decimal Fat)
{
    public static Nutrition Zero => new(0, 0, 0, 0);

    public static Nutrition operator +(Nutrition a, Nutrition b)
        => new(a.Calories + b.Calories, a.Protein + b.Protein, a.Carbs + b.Carbs, a.Fat + b.Fat);

    public Nutrition Scale(decimal factor)
        => new(Calories * factor, Protein * factor, Carbs * factor, Fat * factor);
}
