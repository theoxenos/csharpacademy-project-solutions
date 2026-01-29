using FoodJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodJournal.Data;

public class FoodJournalContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Meal> Meals { get; set; } = null!;
    public DbSet<Food> Foods { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedData.Seed(modelBuilder);
    }
}