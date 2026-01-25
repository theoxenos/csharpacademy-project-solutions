namespace SportsResultsNotifier.Models;

public class Team
{
    public string Name { get; init; } = string.Empty;
    public List<int> Score { get; } = [];
    public int TotalScore => Score.Sum();
    public bool IsWinner { get; set; }
}