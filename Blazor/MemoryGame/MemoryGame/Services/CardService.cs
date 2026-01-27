using MemoryGame.Models;

namespace MemoryGame.Services;

public class CardService
{
    private const int AvailableFaces = 10;

    public List<Card> Create(int amount)
    {
        if (amount < 0 || amount % 2 != 0)
        {
            throw new ArgumentException("Amount must be a non-negative even number.", nameof(amount));
        }

        var cards = new List<Card>(amount);
        int pairs = amount / 2;
        var id = 0;

        for (var i = 0; i < pairs; i++)
        {
            var imageName = $"planet0{i % AvailableFaces}.png";
            cards.Add(new Card(id++, imageName));
            cards.Add(new Card(id++, imageName));
        }

        return cards;
    }
}
