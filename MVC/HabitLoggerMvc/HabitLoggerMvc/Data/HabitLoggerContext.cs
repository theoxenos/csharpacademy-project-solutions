using System.Data;
using HabitLoggerMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitLoggerMvc.Data;

public class HabitLoggerContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitLog> HabitLogs { get; set; }
    public DbSet<HabitUnits> HabitUnits { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Habit to HabitUnits Relationship
        modelBuilder.Entity<Habit>()
            .HasOne<HabitUnits>() // Specifies that Habit has one HabitUnits, without specifying a navigation property
            .WithMany() // Specifies that HabitUnits has many Habit, without specifying a navigation property
            .HasForeignKey(h => h.HabitUnitId); // Specifies the foreign key in the Habit entity

        // Habit to HabitLog Relationship
        modelBuilder.Entity<HabitLog>()
            .HasOne<Habit>() // Specifies that HabitLog has one Habit, without specifying a navigation property
            .WithMany() // Specifies that Habit has many HabitLog, without specifying a navigation property
            .HasForeignKey(hl => hl.HabitId); // Specifies the foreign key in the HabitLog entity
    }

    
    public async Task<IDbConnection> GetConnection()
    {
        var connection = Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        return connection;
    }
}