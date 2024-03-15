using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models;

public class Issue
{
    public int Id { get; set; }
    [DisplayName("Project")]
    public int ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public int UserId { get; set; }
    public User Assignee { get; set; } = default!;
    [StringLength(255, MinimumLength = 3)]
    public string Title { get; set; } = default!;
    [StringLength(4000, MinimumLength = 10)]
    public string Description { get; set; } = default!;
    [DisplayName("Created At")]
    [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:d-M-y HH:mm}")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [DisplayName("Modified At")]
    [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:d-M-y HH:mm}")]
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
    public IssueStatus Status { get; set; } = IssueStatus.Open;
}

public enum IssueStatus
{
    Open,
    Closed
}