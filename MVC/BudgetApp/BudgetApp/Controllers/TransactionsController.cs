using BudgetApp.Data;
using BudgetApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Controllers;

public class TransactionsController(BudgetContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await context.Transactions.OrderBy(t => t.Date).Include(t => t.Category).ToArrayAsync());
    }

    public async Task<IActionResult> Create()
    {
        var vm = await CreateTransactionUpsertViewModel();

        return PartialView("TransactionUpsertModalForm", vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Transaction transaction)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return PartialView("TransactionUpsertModalForm", await CreateTransactionUpsertViewModel(transaction));
        }
        
        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        return PartialView("TransactionsTableRows",
            await context.Transactions.OrderBy(t => t.Date).Include(t => t.Category).ToArrayAsync());
    }

    public async Task<IActionResult> Detail(int id)
    {
        var transaction = await context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound();
        }

        return PartialView("TransactionUpsertModalForm", await CreateTransactionUpsertViewModel(transaction));
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] Transaction transaction)
    {
        if (id != transaction.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return PartialView("TransactionUpsertModalForm", await CreateTransactionUpsertViewModel(transaction));
        }
        
        context.Entry(transaction).State = EntityState.Modified;
        
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TransactionExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return PartialView("TransactionsTableRows",
            await context.Transactions.OrderBy(t => t.Date).Include(t => t.Category).ToArrayAsync());
    }

    private bool TransactionExists(int id)
    {
        return context.Transactions.Any(t => t.Id == id);
    }

    private async Task<TransactionUpsertViewModel> CreateTransactionUpsertViewModel(Transaction? transaction = default)
    {
        return new TransactionUpsertViewModel
        {
            Transaction = transaction ?? new(),
            Categories = new SelectList(await context.Categories.ToListAsync(), nameof(Category.Id), nameof(Category.Name))
        };
    }
}