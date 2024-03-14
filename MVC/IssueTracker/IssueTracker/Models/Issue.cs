using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models;

public class Issue
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public int UserId { get; set; }
    public User Assignee { get; set; } = default!;
    [StringLength(255, MinimumLength = 3)]
    public string Title { get; set; } = default!;
    [StringLength(4000, MinimumLength = 10)]
    public string Description { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
    public IssueStatus Status { get; set; } = IssueStatus.Open;
}

public enum IssueStatus
{
    Open,
    Closed
}