namespace MemoryGame.Models;

public class Card
{
    public Card(int id, string face)
    {
        Id = id;
        Image = face;
    }

    public int Id { get; }
    public bool IsMatched { get; set; }
    public bool IsVisible { get; set; }
    public string Image => IsVisible ? field : "background.png";
}