using System.ComponentModel.DataAnnotations;

namespace HabitLoggerMvc.Models;

public class HabitLog
{
    public int Id { get; set; }
    public int HabitId { get; set; }
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}