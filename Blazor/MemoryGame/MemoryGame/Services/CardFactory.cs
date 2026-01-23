using MemoryGame.Models;

namespace MemoryGame.Services;

public class CardFactory
{
    private const int AvailableFaces = 10;

    public IEnumerable<Card> Create(int amount)
    {
        if (amount < 0 || amount % 2 != 0)
        {
            throw new ArgumentException("Amount must be a non-negative even number.", nameof(amount));
        }

        int pairs = amount / 2;
        var id = 0;

        for (var i = 0; i < pairs; i++)
        {
            var imageName = $"planet0{i % AvailableFaces}.png";
            yield return new Card(id++, imageName);
            yield return new Card(id++, imageName);
        }
    }
}
