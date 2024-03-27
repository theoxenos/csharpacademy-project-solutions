using FoodJournal.Data;
using FoodJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodJournal.Services;

public class FoodService(FoodJournalContext context)
{
    public Task<List<Food>> GetAllFoodsAsync()
    {
        return context.Foods.OrderBy(f => f.Name).ToListAsync();
    }
}