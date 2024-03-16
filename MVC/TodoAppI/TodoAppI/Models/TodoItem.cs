using System.ComponentModel.DataAnnotations;

namespace TodoAppI.Models;

public class TodoItem
{
    public int Id { get; set; }
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
    public bool Completed { get; set; }
}