using FoodJournal.Data;
using FoodJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodJournal.Services;

public class ReportService(FoodJournalContext context)
{
    public Task<List<FoodCountReportViewModel>> CreateReport(DateTime fromDate, DateTime toDate)
    {
        return context.Foods.Select(f =>
            new FoodCountReportViewModel
            {
                Count = f.Meals.Count(m => m.Date >= fromDate && m.Date <= toDate),
                Food = f
            }
        ).ToListAsync();
    }
}