namespace MemoryGame.Models;

public class Score
{
    public DateTime Date { get; set; }
    public Difficulty Difficulty { get; set; }
    public TimeSpan GameDuration { get; set; }
}