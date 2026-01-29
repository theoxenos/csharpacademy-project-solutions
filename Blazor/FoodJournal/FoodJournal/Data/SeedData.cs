using FoodJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodJournal.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Food>().HasData(
            new Food
            {
                Id = 1,
                Name = "Bread",
                Icon = "icons8-kawaii-bread-96.png",
                Calories = 265,
                Protein = 9,
                Carbohydrates = 49,
                Fat = 3
            },
            new Food
            {
                Id = 2,
                Name = "Fruit",
                Icon = "icons8-grapes-96.png",
                Calories = 52,
                Protein = 0.3m,
                Carbohydrates = 14,
                Fat = 0.2m
            },
            new Food
            {
                Id = 3,
                Name = "Tea",
                Calories = 1,
                Protein = 0,
                Carbohydrates = 0.2m,
                Fat = 0
            },
            new Food
            {
                Id = 4,
                Name = "Coffee",
                Icon = "icons8-kawaii-coffee-96.png",
                Calories = 1,
                Protein = 0.1m,
                Carbohydrates = 0,
                Fat = 0
            },
            new Food
            {
                Id = 5,
                Name = "Broccoli",
                Icon = "icons8-kawaii-broccoli-96.png",
                Calories = 34,
                Protein = 2.8m,
                Carbohydrates = 7,
                Fat = 0.4m
            },
            new Food
            {
                Id = 6,
                Name = "Vegetable",
                Icon = "icons8-cute-pumpkin-96.png",
                Calories = 26,
                Protein = 1,
                Carbohydrates = 6.5m,
                Fat = 0.1m
            },
            new Food
            {
                Id = 7,
                Name = "Steak",
                Icon = "icons8-kawaii-steak-96.png",
                Calories = 165,
                Protein = 31,
                Carbohydrates = 0,
                Fat = 3.6m
            },
            new Food
            {
                Id = 8,
                Name = "Rice",
                Icon = "icons8-rice-bowl-96.png",
                Calories = 130,
                Protein = 2.7m,
                Carbohydrates = 28,
                Fat = 0.3m
            },
            new Food
            {
                Id = 9,
                Name = "Egg",
                Icon = "icons8-kawaii-egg-96.png",
                Calories = 155,
                Protein = 13,
                Carbohydrates = 1.1m,
                Fat = 11
            },
            new Food
            {
                Id = 10,
                Name = "Milk",
                Icon = "icons8-milk-carton-96.png",
                Calories = 61,
                Protein = 3.3m,
                Carbohydrates = 4.8m,
                Fat = 3.3m
            },
            new Food
            {
                Id = 11,
                Name = "Salmon",
                Icon = "icons8-salmon-96.png",
                Calories = 208,
                Protein = 20,
                Carbohydrates = 0,
                Fat = 13
            },
            new Food
            {
                Id = 12,
                Name = "Cupcake",
                Icon = "icons8-kawaii-cupcake-96.png",
                Calories = 437,
                Protein = 2,
                Carbohydrates = 25,
                Fat = 5
            }
        );
    }
}