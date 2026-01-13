using System.ComponentModel.DataAnnotations;
using HabitLoggerMvc.Validators;

namespace HabitLoggerMvc.Models;

public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Habit Unit")]
    [IdRequired(0)]
    public int HabitUnitId { get; set; }
}
