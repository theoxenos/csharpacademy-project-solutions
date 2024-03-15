using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models;

public class Project
{
    public int Id { get; set; }
    [StringLength(255, MinimumLength = 3)]
    [DisplayName("Project")]
    public string Name { get; set; } = default!;
    public List<Issue> Issues { get; set; } = [];
}