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
            }
        );
    }
}