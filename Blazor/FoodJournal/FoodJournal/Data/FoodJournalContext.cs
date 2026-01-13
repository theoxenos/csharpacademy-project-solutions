using FoodJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodJournal.Data;

public class FoodJournalContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Meal> Meals { get; set; } = default!;
    public DbSet<Food> Foods { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Food>().HasData(
            new Food
            {
                Id = 1,
                Name = "Bread",
                Icon = "icons8-kawaii-bread-96.png"
            },
            new Food
            {
                Id = 2,
                Name = "Fruit",
                Icon = "icons8-grapes-96.png"
            },
            new Food
            {
                Id = 3,
                Name = "Tea"
            },
            new Food
            {
                Id = 4,
                Name = "Coffee",
                Icon = "icons8-kawaii-coffee-96.png"
            },
            new Food
            {
                Id = 5,
                Name = "Broccoli",
                Icon = "icons8-kawaii-broccoli-96.png"
            },
            new Food
            {
                Id = 6,
                Name = "Vegetable",
                Icon = "icons8-cute-pumpkin-96.png"
            }
        );
    }
}