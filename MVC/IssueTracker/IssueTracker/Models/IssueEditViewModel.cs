using Microsoft.AspNetCore.Mvc.Rendering;

namespace IssueTracker.Models;

public class IssueEditViewModel
{
    public Issue Issue { get; set; } = default!;
    public SelectList Users { get; set; } = default!;
    public SelectList Projects { get; set; } = default!;
    public SelectList Statuses { get; set; } = default!;
}