namespace MemoryGame.Models;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public static class DifficultyExtensions
{
    private static Dictionary<Difficulty, string> DifficultyNames => new()
    {
        { Difficulty.Easy, "Cadet" },
        { Difficulty.Medium, "Commander" },
        { Difficulty.Hard, "Legend" }
    };

    public static string GetDifficultyName(this Difficulty difficulty)
    {
        DifficultyNames.TryGetValue(difficulty, out string? name);
        return name ?? throw new ArgumentException("Difficulty not found", nameof(difficulty));
    }
}
