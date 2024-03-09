using System.Text;
using SportsResultsNotifier.Models;
using SportsResultsNotifier.Services;

namespace SportsResultsNotifier.Controllers;

public class MainController(MailService mailService, ScraperService scraperService)
{
    public void Start()
    {
        var result = scraperService.ScrapeSite();
        var body = ConvertMatchesToBody(result);
        mailService.SendMail(body);
    }

    private string ConvertMatchesToBody(List<GameMatch> results)
    {
        var body = new StringBuilder();
        foreach (var match in results)
        {
            var teamAName = match.TeamA.IsWinner ? $"**{match.TeamA.Name}**" : match.TeamA.Name;
            var teamBName = match.TeamB.IsWinner ? $"**{match.TeamB.Name}**" : match.TeamB.Name;
            body.Append($"# {teamAName} vs {teamBName}{Environment.NewLine}");
            
            var teamAScore =$"{string.Join("\t", match.TeamA.Score)}\t{match.TeamA.TotalScore}{Environment.NewLine}";
            var teamBScore = $"{string.Join("\t", match.TeamB.Score)}\t{match.TeamB.TotalScore}{Environment.NewLine}";
            body.Append(teamAScore);
            body.Append(teamBScore);
        }
        
        return body.ToString();
    }
}