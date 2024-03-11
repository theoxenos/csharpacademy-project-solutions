using System.ComponentModel.DataAnnotations;

namespace HabitLoggerMvc.Models;

public class HabitLog
{
    public int Id { get; set; }
    public int HabitId { get; set; }
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;
    public int Quantity { get; set; }
}