namespace MemoryGame.Models;

public class Card
{
    public int Id { get; set; }
    public bool IsMatched { get; set; }
    public bool IsVisible { get; set; }
    public string Image { get; set; } = string.Empty;
}