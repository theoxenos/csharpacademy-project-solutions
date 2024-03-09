using Microsoft.Extensions.Logging;
using SportsResultsNotifier.Services;

namespace SportsResultsNotifier.Controllers;

public class MainController(MailService mailService, ScraperService scraperService)
{
    public void Start()
    {
    }
}