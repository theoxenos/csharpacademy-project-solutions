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
        TransactionIndexViewModel vm = new()
        {
            Transactions = await context.Transactions.OrderBy(t => t.Date).Include(t => t.Category).ToArrayAsync(),
            TransactionUpsertViewModel = await CreateTransactionUpsertViewModel(),
            Categories = await context.Categories.ToListAsync()
        };
        return View(vm);
    }

    public async Task<IActionResult> Filter(TransactionIndexViewModel indexViewModel)
    {
        string searchTerm = indexViewModel.TransactionFilter.Trim();
        IQueryable<Transaction> filteredTransactions = context.Transactions.Include(t => t.Category).AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            filteredTransactions = filteredTransactions.Where(t =>
                EF.Functions.Like(t.Comment, $"%{searchTerm}%")
            );
        }

        if (indexViewModel.DateFilter.HasValue)
        {
            filteredTransactions = filteredTransactions.Where(t => t.Date.Date == indexViewModel.DateFilter.Value.Date);
        }

        if (indexViewModel.CategoryFilter != 0)
        {
            filteredTransactions = filteredTransactions.Where(t => t.CategoryId == indexViewModel.CategoryFilter);
        }

        return PartialView("TransactionsTableRows", await filteredTransactions.OrderBy(t => t.Date).ToArrayAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromBody] Transaction transaction)
    {
        ModelState.Remove(nameof(Transaction.Category));
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        return PartialView("TransactionsTableRows",
            await context.Transactions.OrderBy(t => t.Date).Include(t => t.Category).ToArrayAsync());
    }

    public async Task<IActionResult> Detail(int id)
    {
        Transaction? transaction = await context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound();
        }

        return Ok(transaction);
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, [FromBody] Transaction transaction)
    {
        if (id != transaction.Id)
        {
            return BadRequest();
        }

        ModelState.Remove(nameof(Transaction.Category));

        if (!ModelState.IsValid)
        {
            return BadRequest();
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

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Transaction? transaction = await context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return BadRequest();
        }

        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync();

        return PartialView("TransactionsTableRows",
            await context.Transactions.OrderBy(t => t.Date).Include(t => t.Category).ToArrayAsync());
    }

    private bool TransactionExists(int id) => context.Transactions.Any(t => t.Id == id);

    private async Task<TransactionUpsertViewModel>
        CreateTransactionUpsertViewModel(Transaction? transaction = null) =>
        new()
        {
            Transaction = transaction ?? new Transaction(),
            Categories = new SelectList(await context.Categories.ToListAsync(), nameof(Category.Id),
                nameof(Category.Name))
        };
}
