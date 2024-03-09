namespace SportsResultsNotifier.Models;

public class Team
{
    public string Name { get; set; }
    public List<int> Score { get; set; } = [];
    public int TotalScore => Score.Sum();
    public bool IsWinner { get; set; }
}