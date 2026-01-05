using CodingTracker.Data; // Database wrapper
using CodingTracker.Models; // SessionLog model
using CodingTracker.Repositories;
using Microsoft.Data.Sqlite; // SessionLogRepository implementation

namespace CodingTracker.Tests.IntegrationTests;

public sealed class SessionLogRepositoryTests : IDisposable
{
    private const string TestDbPath = "sessionlog-test.db";
    private readonly SessionLogRepository repo;
    private Database db;

    private static readonly DateTime FixedTimestamp = new DateTime(2023, 01, 01, 12, 00, 00, DateTimeKind.Utc);

    public SessionLogRepositoryTests()
    {
        db = new Database($"Data Source={TestDbPath}");
        db.Initialize(); // creates tables, etc.
        repo = new SessionLogRepository(db);
    }

    [Fact]
    public void GetAllSessionLogs_ReturnsAllSessionLogs()
    {
        // Act
        var sessions = repo.GetAllSessionLogs();

        // Assert
        Assert.Equal(db.SeedLogs.Length, sessions.Count);
        Assert.Equal(db.SeedLogs[0].Duration, sessions[0].Duration);
        Assert.Equal(db.SeedLogs[0].StartTime, sessions[0].StartTime);
        Assert.Equal(db.SeedLogs[0].EndTime, sessions[0].EndTime);
    }

    [Fact]
    public void CreateLog_AddsEntryAndCanBeRetrieved()
    {
        // Arrange
        var sessionId = 4;
        var before = repo.GetAllSessionLogs();
        var newSession = new SessionLog
        {
            SessionId = sessionId,
            StartTime = TimeOnly.FromDateTime(FixedTimestamp),
            EndTime = TimeOnly.FromDateTime(FixedTimestamp).AddHours(1),
            Duration = TimeSpan.FromHours(1),
        };

        // Act
        repo.CreateLog(newSession);
        var after = repo.GetAllSessionLogs();
        var created = after.OrderByDescending(l => l.Id).FirstOrDefault(s =>
            s.SessionId == sessionId &&
            s.StartTime == newSession.StartTime &&
            s.EndTime == newSession.EndTime);

        // Assert
        Assert.Equal(before.Count + 1, after.Count);
        Assert.NotNull(created);
        Assert.Contains(after,
            l => l.Id == created.Id &&
                 l.SessionId == sessionId &&
                 l.StartTime == newSession.StartTime &&
                 l.EndTime == newSession.EndTime &&
                 l.Duration == newSession.Duration);
    }

    [Fact]
    public void UpdateLog_ChangesFieldsWithoutAffectingOtherRows()
    {
        // Arrange
        var toUpdateLog = db.SeedLogs[0];
        var notUpdatedLog = db.SeedLogs[1];

        // Act
        toUpdateLog.EndTime = toUpdateLog.EndTime.AddHours(1);
        toUpdateLog.Duration = toUpdateLog.EndTime - toUpdateLog.StartTime;
        repo.UpdateSessionLog(toUpdateLog);

        // Assert
        var allAfter = repo.GetAllSessionLogs();
        Assert.Equal(db.SeedLogs.Length, allAfter.Count);

        // Should have with same values
        Assert.Contains(allAfter,
            l => l.SessionId == toUpdateLog.SessionId &&
                 l.EndTime == toUpdateLog.EndTime &&
                 l.Duration == toUpdateLog.Duration &&
                 l.StartTime == toUpdateLog.StartTime);

        // Should have with updated values
        Assert.Contains(allAfter,
            l => l.SessionId == notUpdatedLog.SessionId &&
                 l.EndTime == notUpdatedLog.EndTime &&
                 l.Duration == notUpdatedLog.Duration &&
                 l.StartTime == notUpdatedLog.StartTime);
    }

    [Fact]
    public void DeleteLog_RemovesEntryAndDecrementsCount()
    {
        // Arrange
        var logA = db.SeedLogs[0];
        var logB = db.SeedLogs[1];

        var before = repo.GetAllSessionLogs();

        // Act
        repo.DeleteSessionLog(logA);

        // Assert
        var after = repo.GetAllSessionLogs();
        Assert.Equal(before.Count - 1, after.Count);
        Assert.DoesNotContain(after, l => l.Id == logA.Id);
        // The other log must still be present
        Assert.Contains(after, l => l.Id == logB.Id);
    }

    [Fact]
    public void GetLogsBySessionId_ReturnsOnlyMatchingRows()
    {
        // Arrange
        var sessionId = 1;
        var sessionOneLogs = db.SeedLogs.Where(l => l.SessionId == sessionId).ToList();
    
        // Act
        var logsForFirstSession = repo.GetLogsBySessionId(sessionId).ToList();
    
        // Assert
        Assert.Equal(sessionOneLogs.Count, logsForFirstSession.Count);
        Assert.All(logsForFirstSession, l => Assert.Equal(sessionId, l.SessionId));
        Assert.Contains(logsForFirstSession, l => l.Id == sessionOneLogs.First().Id);
        Assert.Contains(logsForFirstSession, l => l.Id == sessionOneLogs.Skip(1).First().Id);
        Assert.DoesNotContain(logsForFirstSession, l => l.Id == db.SeedLogs[2].Id);
    }

    [Fact]
    public void Delete_NonExistentLog_ThrowsOrIgnoresGracefully()
    {
        // Arrange
        var wrongSession = new SessionLog
        {
            Id = 9999,
            SessionId = 1,
            StartTime = TimeOnly.FromDateTime(FixedTimestamp),
            EndTime = TimeOnly.FromDateTime(FixedTimestamp).AddHours(1),
            Duration = TimeSpan.FromHours(1)
        };
    
        // Act & Assert
        // Adjust the expected exception type to whatever your repo throws.
        // If your implementation silently ignores missing rows, replace the Assert.Throws with a simple call.
        Assert.Throws<InvalidOperationException>(() => repo.DeleteSessionLog(wrongSession));
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        // Remove the temporary file.
        if (File.Exists(TestDbPath))
            File.Delete(TestDbPath);
    }
}