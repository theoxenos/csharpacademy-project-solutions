using MemoryGame.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MemoryGame.Services;

public class ScoreService(ProtectedLocalStorage localStorage)
{
    private const string StorageKey = "scores";

    private readonly List<Score> _seedData =
    [
        new(GameDuration: TimeSpan.FromSeconds(5), Difficulty: Difficulty.Easy,
            Date: new DateTime(2025, 12, 1).AddHours(12)),
        new(GameDuration: TimeSpan.FromSeconds(10), Difficulty: Difficulty.Medium,
            Date: new DateTime(2025, 12, 21).AddHours(12)),
        new(GameDuration: TimeSpan.FromSeconds(15), Difficulty: Difficulty.Hard,
            Date: new DateTime(2025, 12, 21).AddHours(12).AddMinutes(12)),
    ];

    public async Task<List<Score>> GetScores()
    {
        ProtectedBrowserStorageResult<List<Score>> result = await localStorage.GetAsync<List<Score>>(StorageKey);
        return result.Value ?? _seedData;
    }

    public async Task SaveScore(Score score)
    {
        List<Score> scores = await GetScores();
        scores.Add(score);
        scores.Sort((score1, score2) => score1.GameDuration.CompareTo(score2.GameDuration));
        await localStorage.SetAsync(StorageKey, scores);
    }
}
