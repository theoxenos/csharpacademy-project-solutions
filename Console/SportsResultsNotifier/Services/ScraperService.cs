using HtmlAgilityPack;
using SportsResultsNotifier.Models;

namespace SportsResultsNotifier.Services;

public class ScraperService
{
    public List<GameMatch> ScrapeSite()
    {
        const string url = "https://www.basketball-reference.com/boxscores/";
        var web = new HtmlWeb();
        HtmlDocument doc = web.Load(url);
        var matches = new List<GameMatch>();

        foreach (HtmlNode summary in doc.DocumentNode.SelectNodes("//div[contains(@class, 'game_summary')]"))
        {
            var match = new GameMatch();

            HtmlNode teams = summary.SelectSingleNode(".//table[2]/tbody");
            match.TeamA = GetTeamFromNode(teams.SelectSingleNode(".//tr[1]"));
            match.TeamB = GetTeamFromNode(teams.SelectSingleNode(".//tr[2]"));

            match.TeamA.IsWinner = match.TeamA.TotalScore > match.TeamB.TotalScore;
            match.TeamB.IsWinner = match.TeamB.TotalScore > match.TeamA.TotalScore;

            matches.Add(match);
        }

        return matches;
    }

    private Team GetTeamFromNode(HtmlNode teamNode)
    {
        var team = new Team
        {
            Name = teamNode.SelectSingleNode(".//a").InnerText
        };

        HtmlNodeCollection rounds = teamNode.SelectNodes("./td[@class='center']");
        foreach (HtmlNode roundScore in rounds) team.Score.Add(int.Parse(roundScore.InnerText));

        return team;
    }
}