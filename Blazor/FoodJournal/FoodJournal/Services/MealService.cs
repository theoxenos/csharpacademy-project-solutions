using FoodJournal.Data;
using FoodJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodJournal.Services;

public class MealService(FoodJournalContext context)
{
    public async Task AddMeal(Meal meal)
    {
        context.Meals.Add(meal);
        await context.SaveChangesAsync();
    }

    public Task<List<Meal>> GetAllAsync()
    {
        return context.Meals.Include(m => m.Foods).ToListAsync();
    }
}