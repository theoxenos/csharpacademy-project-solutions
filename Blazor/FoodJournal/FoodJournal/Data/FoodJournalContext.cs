using FoodJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodJournal.Data;

public class FoodJournalContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Meal> Meals { get; set; } = default!;
    public DbSet<Food> Foods { get; set; } = default!;
}