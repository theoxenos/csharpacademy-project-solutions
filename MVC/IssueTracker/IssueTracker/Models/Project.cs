using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models;

public class Project
{
    public int Id { get; set; }
    [StringLength(255, MinimumLength = 3)]
    public string Name { get; set; } = default!;
    public List<Issue> Issues { get; set; } = [];
}