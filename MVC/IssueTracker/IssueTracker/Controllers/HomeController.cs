using System.Diagnostics;
using IssueTracker.Data;
using Microsoft.AspNetCore.Mvc;
using IssueTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Controllers;

public class HomeController(IssueTrackerContext context) : Controller
{
    public IActionResult Index()
    {
        return View(context.Issues.Include(i => i.Project).Include(i => i.Assignee));
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