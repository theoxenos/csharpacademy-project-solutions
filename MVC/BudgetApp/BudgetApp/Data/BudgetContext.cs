using BudgetApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Data;

public class BudgetContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Transaction> Transactions { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Transactions)
            .WithOne(e => e.Category)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasPrecision(18, 2);
        
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Rent"
            },
            new Category
            {
                Id = 2,
                Name = "Insurance"
            },
            new Category
            {
                Id = 3,
                Name = "Car"
            },
            new Category
            {
                Id = 4,
                Name = "Groceries"
            }
        );

        modelBuilder.Entity<Transaction>().HasData(
            new Transaction
            {
                Id = 1,
                Amount = 420.69m,
                CategoryId = 1,
                Date = new DateTime(2024, 3, 1, 12, 0, 0),
                Comment = "Rent for March"
            },
            new Transaction
            {                
                Id = 2,
                Amount = 36.99m,
                CategoryId = 4,
                Date = DateTime.Today.AddHours(19).AddMinutes(51),
                Comment = "Groceries for dinner"
            },
            new Transaction
            {
                Id = 3,
                Amount = 5,
                CategoryId = 3,
                Date = DateTime.Now,
                Comment = "Car wash"
            }
        );
    }
}