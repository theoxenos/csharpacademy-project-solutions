using HabitLoggerMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Data;

public class HabitLoggerContext : DbContext
{
    public HabitLoggerContext(DbContextOptions<HabitLoggerContext> options)
        : base(options)
    {
    }

    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitUnit> HabitUnits { get; set; }
    public DbSet<HabitLog> HabitLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<HabitUnit>().HasIndex(u => u.Name).IsUnique();
        modelBuilder.Entity<HabitUnit>(b =>
        {
            b.HasData
            (
                new HabitUnit { Id = 1, Name = "Medium glass" },
                new HabitUnit { Id = 2, Name = "Small glass" },
                new HabitUnit { Id = 3, Name = "Meters" },
                new HabitUnit { Id = 4, Name = "Minutes" },
                new HabitUnit { Id = 5, Name = "Pages" }
            );
        });

        modelBuilder.Entity<Habit>().HasIndex(h => h.Name).IsUnique();
        modelBuilder.Entity<Habit>(b =>
        {
            b.HasData(
                new Habit { Id = 1, Name = "Drinking water", HabitUnitId = 1 },
                new Habit { Id = 2, Name = "Drinking fruit sap", HabitUnitId = 2 },
                new Habit { Id = 3, Name = "Walking", HabitUnitId = 3 },
                new Habit { Id = 4, Name = "Meditation", HabitUnitId = 4 },
                new Habit { Id = 5, Name = "Reading", HabitUnitId = 5 }
            );
        });

        modelBuilder.Entity<HabitLog>(b => b.HasData(
            new HabitLog { Id = 1, HabitId = 1, Date = new DateTime(2023, 01, 01), Quantity = 8 },
            new HabitLog { Id = 2, HabitId = 2, Date = new DateTime(2023, 01, 02), Quantity = 5 },
            new HabitLog { Id = 3, HabitId = 3, Date = new DateTime(2023, 01, 03), Quantity = 3 },
            new HabitLog { Id = 4, HabitId = 1, Date = new DateTime(2023, 01, 04), Quantity = 7 },
            new HabitLog { Id = 5, HabitId = 2, Date = new DateTime(2023, 01, 05), Quantity = 4 },
            new HabitLog { Id = 6, HabitId = 4, Date = new DateTime(2023, 01, 06), Quantity = 30 },
            new HabitLog { Id = 7, HabitId = 5, Date = new DateTime(2023, 01, 07), Quantity = 150 }
        ));
    }
}
