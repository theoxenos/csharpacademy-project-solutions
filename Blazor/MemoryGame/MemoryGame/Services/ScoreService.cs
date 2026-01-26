using MemoryGame.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MemoryGame.Services;

public class ScoreService(ProtectedLocalStorage localStorage)
{
    private const string StorageKey = "scores";

    public async Task<List<Score>> GetScores()
    {
        ProtectedBrowserStorageResult<List<Score>> result = await localStorage.GetAsync<List<Score>>(StorageKey);
        // result.Value?.Sort((score1, score2) => score1.GameDuration.CompareTo(score2.GameDuration));
        return result.Value ?? [];
    }

    public async Task SaveScore(Score score)
    {
        List<Score> scores = await GetScores();
        scores.Add(score);
        scores.Sort((score1, score2) => score1.GameDuration.CompareTo(score2.GameDuration));
        await localStorage.SetAsync(StorageKey, scores);
    }
}
