using IssueTracker.Data;
using IssueTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Controllers;

public class IssueController(IssueTrackerContext context) : Controller
{
    public async Task<IActionResult> Index(int? id)
    {
        if (!id.HasValue)
            return NotFound();

        var issue = await context.Issues.Include(i => i.Assignee).Include(i => i.Project)
            .FirstOrDefaultAsync(i => i.Id == id);
        
        if (issue == null)
            return NotFound();
        
        return View(issue);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (!id.HasValue)
            return NotFound();

        var issue = await context.Issues.Include(i => i.Assignee).Include(i => i.Project)
            .FirstOrDefaultAsync(i => i.Id == id);
        
        if (issue == null)
            return NotFound();
        var vm = await CreateIssueEditViewModel(issue);

        return View(vm);
    }

    private async Task<IssueEditViewModel> CreateIssueEditViewModel(Issue issue)
    {
        var issueStatuses = new SelectList(Enum.GetValues<IssueStatus>());
        var users = new SelectList(await context.Users.ToListAsync(), "Id", "Name", issue.UserId);
        var projects = new SelectList(await context.Projects.ToListAsync(), nameof(Project.Id), nameof(Project.Name),
            issue.ProjectId);
        var vm = new IssueEditViewModel
        {
            Issue = issue,
            Users = users,
            Projects = projects,
            Statuses = issueStatuses
        };
        return vm;
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ProjectId,UserId,ModifiedAt,Status", Prefix = "Issue")] Issue issue)
    {
        if (id != issue.Id || id != issue.Id)
            return NotFound();

        ModelState.Remove($"{nameof(Issue)}.{nameof(Issue.Project)}");
        ModelState.Remove($"{nameof(Issue)}.{nameof(Issue.Assignee)}");
        
        if (!ModelState.IsValid)
        {
            return View(await CreateIssueEditViewModel(issue));
        }

        try
        {
            issue.ModifiedAt = DateTime.Now;
            context.Issues.Update(issue);
            context.Entry(issue).Property(i => i.CreatedAt).IsModified = false;
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Issues.Any(i => i.Id == id))
                return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Index), new { id = issue.Id });
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (!id.HasValue)
            return NotFound();

        var issue = await context.Issues.Include(i => i.Assignee).Include(i => i.Project)
            .FirstOrDefaultAsync(i => i.Id == id);
        if (issue == null)
            return NotFound();
        
        return View(issue);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var issue = await context.Issues.FindAsync(id);
        if (issue == null)
            return NotFound();
        
        try
        {
            context.Issues.Remove(issue);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            if (!context.Issues.Any(i => i.Id == id))
                return NotFound();
            
            throw;
        }

        return RedirectToRoute(new { Controller = "Home", Action = "Index" });
    }

    public async Task<IActionResult> Create()
    {
        return View(await CreateIssueEditViewModel(new Issue()));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Issue issue)
    {
        try
        {
            await context.Issues.AddAsync(issue);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return RedirectToAction(nameof(Index), new { issue.Id });
    }
}