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
        return context.Meals.OrderByDescending(m => m.Date).Include(m => m.Foods).ToListAsync();
    }

    public Task<List<Meal>> GetMealsBasedOnSearchAsync(MealSearchViewModel searchVm)
    {
        IQueryable<Meal> query = context.Meals;

        if (!string.IsNullOrEmpty(searchVm.SearchTerm))
        {
            var searchTerm = searchVm.SearchTerm.Trim();
            query = context.Meals.Where(m =>
                EF.Functions.Like(m.Name, $"%{searchTerm}%")
                || m.Foods.Any(f => EF.Functions.Like(f.Name, $"%{searchTerm}%")));
        }

        if (searchVm.MealType != null) query = query.Where(m => m.MealType == searchVm.MealType);

        if (searchVm.Date != null) query = query.Where(m => m.Date == searchVm.Date);

        return query.OrderByDescending(m => m.Date).Include(m => m.Foods).ToListAsync();
    }
}