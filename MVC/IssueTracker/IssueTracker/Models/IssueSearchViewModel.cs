using Microsoft.AspNetCore.Mvc.Rendering;

namespace IssueTracker.Models;

public class IssueSearchViewModel
{
    public List<Issue>? Issues { get; set; }
    public SelectList? Statuses { get; set; }
    public IssueStatus Status { get; set; } = IssueStatus.Open;
    public string? SearchTerm { get; set; }
}