using IssueTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Data;

public class IssueTrackerContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Issue> Issues { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Jane Doe", Email = "janedoe@example.com" },
            new User { Id = 2, Name = "John Doe", Email = "johndoe@example.com" }
        );

        modelBuilder.Entity<Project>().HasData(
            new Project { Id = 1, Name = "Project A" },
            new Project { Id = 2, Name = "Project B" }
        );

        modelBuilder.Entity<Issue>().HasData(
            new Issue { 
                Id = 1, 
                ProjectId = 1, 
                UserId = 1, 
                Title = "Initial Issue A", 
                Description = "This is a detailed description of the initial issue in Project A.", 
                CreatedAt = new DateTime(2024,03,14, 14, 35 ,00), 
                ModifiedAt = DateTime.Now.AddHours(-2) 
            },
            new Issue { 
                Id = 2, 
                ProjectId = 2, 
                UserId = 2, 
                Title = "Initial Issue B", 
                Description = "This is a detailed description of the initial issue in Project B.", 
                CreatedAt = DateTime.Now, 
                ModifiedAt = DateTime.Now 
            }
        );
    }
}