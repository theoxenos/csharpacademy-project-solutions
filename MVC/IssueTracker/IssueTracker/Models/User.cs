using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models;

public class User
{
    public int Id { get; set; }
    [StringLength(255, MinimumLength = 3)]
    [DisplayName("Assignee")]
    public string Name { get; set; } = default!;
    [StringLength(255, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    public string Email { get; set; } = default!;
}