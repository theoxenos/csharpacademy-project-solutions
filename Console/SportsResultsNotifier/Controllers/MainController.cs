using System.Text;
using SportsResultsNotifier.Models;
using SportsResultsNotifier.Services;

namespace SportsResultsNotifier.Controllers;

public class MainController(MailService mailService, ScraperService scraperService)
{
    public void Start()
    {
        List<GameMatch> result = scraperService.ScrapeSite();
        string body = ConvertMatchesToBody(result);
        mailService.SendMail(body);
    }

    private string ConvertMatchesToBody(List<GameMatch> results)
    {
        var body = new StringBuilder();
        body.Append("<html><body>");

        foreach (GameMatch match in results)
        {
            // Determine formatted team names with bold for winners
            string teamANameFormatted =
                match.TeamA.IsWinner ? $"<strong>{match.TeamA.Name}</strong>" : match.TeamA.Name;
            string teamBNameFormatted =
                match.TeamB.IsWinner ? $"<strong>{match.TeamB.Name}</strong>" : match.TeamB.Name;

            // Start the table and add the match heading
            body.Append($"<h2>{teamANameFormatted} vs {teamBNameFormatted}</h2>");
            body.Append("<table border='1'><tr><th>Team</th><th>1</th><th>2</th><th>3</th><th>4</th>");

            // Add an overtime header if needed
            bool hasOt = match.TeamA.Score.Count > 4;
            if (hasOt) body.Append("<th>OT</th>");

            // Continue with the Total column header
            body.Append("<th>Total</th></tr>");

            // Add the scores for team A
            body.Append($"<tr><td>{teamANameFormatted}</td>");
            foreach (int score in match.TeamA.Score) body.Append($"<td>{score}</td>");
            body.Append($"<td>{match.TeamA.TotalScore}</td></tr>");

            // Add the scores for team B
            body.Append($"<tr><td>{teamBNameFormatted}</td>");
            foreach (int score in match.TeamB.Score) body.Append($"<td>{score}</td>");
            body.Append($"<td>{match.TeamB.TotalScore}</td></tr>");

            // Close the table for this match
            body.Append("</table>");
        }

        body.Append("</body></html>");

        return body.ToString();
    }
}