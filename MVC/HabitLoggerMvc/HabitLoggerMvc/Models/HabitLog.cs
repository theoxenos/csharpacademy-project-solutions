namespace HabitLoggerMvc.Models;

public class HabitLog
{
    public int Id { get; set; }
    public int HabitId { get; set; }
    public DateOnly Date { get; set; }
    public int Quantity { get; set; }
}