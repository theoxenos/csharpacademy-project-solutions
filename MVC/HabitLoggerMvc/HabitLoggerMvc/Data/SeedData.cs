using HabitLoggerMvc.Models;

namespace HabitLoggerMvc.Data;

public class SeedData
{
    public HabitUnit[] HabitUnitSeeds { get; } =
    [
        new() { Id = 1, Name = "Medium glass" },
        new() { Id = 2, Name = "Small glass" },
        new() { Id = 3, Name = "Meters" },
        new() { Id = 4, Name = "Minutes" },
        new() { Id = 5, Name = "Pages" },
    ];

    public Habit[] HabitSeeds { get; } =
    [
        new() { Id = 1, Name = "Drinking water", HabitUnitId = 1 },
        new() { Id = 2, Name = "Drinking fruit sap", HabitUnitId = 2 },
        new() { Id = 3, Name = "Walking", HabitUnitId = 3 },
        new() { Id = 4, Name = "Meditation", HabitUnitId = 4 },
        new() { Id = 5, Name = "Reading", HabitUnitId = 5 },
    ];

    public HabitLog[] HabitLogs { get; } =
    [
        new() { HabitId = 1, Date = new DateTime(2023, 01, 01), Quantity = 8 },
        new() { HabitId = 2, Date = new DateTime(2023, 01, 02), Quantity = 5 },
        new() { HabitId = 3, Date = new DateTime(2023, 01, 03), Quantity = 3 },
        new() { HabitId = 1, Date = new DateTime(2023, 01, 04), Quantity = 7 },
        new() { HabitId = 2, Date = new DateTime(2023, 01, 05), Quantity = 4 },
        new() { HabitId = 4, Date = new DateTime(2023, 01, 06), Quantity = 30 },
        new() { HabitId = 5, Date = new DateTime(2023, 01, 07), Quantity = 150 },
    ];
}