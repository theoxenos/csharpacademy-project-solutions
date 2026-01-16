using System.Diagnostics;
using IssueTracker.Data;
using Microsoft.AspNetCore.Mvc;
using IssueTracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Controllers;

public class HomeController(IssueTrackerContext context) : Controller
{
    public async Task<IActionResult> Index(string? searchTerm, IssueStatus status)
    {   
        var issues = context.Issues.Include(i => i.Project).Include(i => i.Assignee)
            .Where(i => string.IsNullOrEmpty(searchTerm) || i.Title.Contains(searchTerm) && i.Status == status);
        var vm = new IssueSearchViewModel
        {
            Issues = await issues.ToListAsync(),
            SearchTerm = searchTerm,
            Statuses = new SelectList(Enum.GetValues<IssueStatus>())
        };
        
        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}