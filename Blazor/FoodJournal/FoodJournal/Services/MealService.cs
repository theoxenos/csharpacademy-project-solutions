using FoodJournal.Data;
using FoodJournal.Models;

namespace FoodJournal.Services;

public class MealService(FoodJournalContext context)
{
    public async Task AddMeal(Meal meal)
    {
        context.Meals.Add(meal);
        await context.SaveChangesAsync();
    }
}