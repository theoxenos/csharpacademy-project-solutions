using System.Data;
using HabitLoggerMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Data;

public class HabitLoggerContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitLog> HabitLogs { get; set; }
    public DbSet<HabitUnit> HabitUnits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Relationships

        // Habit to HabitUnits Relationship
        modelBuilder.Entity<Habit>()
            .HasOne<HabitUnit>()
            .WithMany()
            .HasForeignKey(h => h.HabitUnitId);

        // Habit to HabitLog Relationship
        modelBuilder.Entity<HabitLog>()
            .HasOne<Habit>()
            .WithMany()
            .HasForeignKey(hl => hl.HabitId);

        #endregion

        SeedHabitData(modelBuilder);
        SeedHabitUnitData(modelBuilder);
        SeedHabitLogData(modelBuilder);
    }

    private void SeedHabitLogData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HabitLog>().HasData(
            new HabitLog { Id = 1, HabitId = 1, Date = new DateOnly(2023, 1, 1), Quantity = 8 },
            new HabitLog { Id = 2, HabitId = 2, Date = new DateOnly(2023, 1, 2), Quantity = 5 },
            new HabitLog { Id = 3, HabitId = 3, Date = new DateOnly(2023, 1, 3), Quantity = 3 },
            new HabitLog { Id = 4, HabitId = 1, Date = new DateOnly(2023, 1, 4), Quantity = 7 },
            new HabitLog { Id = 5, HabitId = 2, Date = new DateOnly(2023, 1, 5), Quantity = 4 },
            new HabitLog
                { Id = 6, HabitId = 4, Date = new DateOnly(2023, 1, 6), Quantity = 30 }, 
            new HabitLog
                { Id = 7, HabitId = 5, Date = new DateOnly(2023, 1, 7), Quantity = 150 } 
        );
    }

    private void SeedHabitUnitData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HabitUnit>().HasData
        (
            new HabitUnit { Id = 1, Name = "Medium glass" },
            new HabitUnit { Id = 2, Name = "Small glass" },
            new HabitUnit { Id = 3, Name = "Meters" },
            new HabitUnit { Id = 4, Name = "Minutes" },
            new HabitUnit { Id = 5, Name = "Pages" }
        );
    }

    private void SeedHabitData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Habit>().HasData(
            new Habit { Id = 1, Name = "Drinking water", HabitUnitId = 1 },
            new Habit { Id = 2, Name = "Drinking fruit sap", HabitUnitId = 2 },
            new Habit { Id = 3, Name = "Walking", HabitUnitId = 3 },
            new Habit { Id = 4, Name = "Meditation", HabitUnitId = 4 },
            new Habit { Id = 5, Name = "Reading", HabitUnitId = 5 }
        );
    }

    public async Task<IDbConnection> GetConnection()
    {
        var connection = Database.GetDbConnection();
        if (connection.State != ConnectionState.Open) await connection.OpenAsync();

        return connection;
    }
}