using System.ComponentModel.DataAnnotations;
using HabitLoggerMvc.Validators;

namespace HabitLoggerMvc.Models;

public class HabitLog
{
    public int Id { get; set; }
    public int HabitId { get; set; }

    [DataType(DataType.Date)] public DateTime Date { get; set; } = DateTime.Today;

    [IsPositive] public int Quantity { get; set; }
}
