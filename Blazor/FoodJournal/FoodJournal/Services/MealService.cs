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
            var searchTerm = searchVm.SearchTerm.Trim().ToLower();
            query = context.Meals.Where(m => m.Name.ToLower().Contains(searchTerm) || m.Foods.Any(f => f.Name.ToLower().Contains(searchTerm)));
        }

        if (searchVm.MealType != null)
        {
            query = query.Where(m => m.MealType == searchVm.MealType);
        }

        if (searchVm.Date != null)
        {
            query = query.Where(m => m.Date == searchVm.Date);
        }

        return query.OrderByDescending(m => m.Date).Include(m => m.Foods).ToListAsync();
        //
        // return context.Meals.Include(m => m.Foods)
        //     .Where(m => 
        //         (searchVm.Date == null || m.Date == searchVm.Date)
        //         && (searchVm.MealType == null || m.MealType == searchVm.MealType )
        //         && (string.IsNullOrEmpty(searchVm.SearchTerm) || m.Name.Contains(searchVm.SearchTerm))
        //         && (string.IsNullOrEmpty(searchVm.SearchTerm) || m.Foods.Any(f => f.Name.Contains(searchVm.SearchTerm)))
        //     )
        //     .ToListAsync();
    }
}