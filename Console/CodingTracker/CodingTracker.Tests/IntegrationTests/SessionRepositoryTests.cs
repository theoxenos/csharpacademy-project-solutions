using CodingTracker.Data;
using CodingTracker.Exceptions;
using CodingTracker.Models;
using CodingTracker.Repositories;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Tests.IntegrationTests;

public class SessionRepositoryTests: IDisposable
{
    private readonly DateOnly FixedDate = new(2026, 01, 01);
    private readonly SessionRepository sessionRepository;
    private const string testDbPath = "test.db";

    public SessionRepositoryTests()
    {
        var db = new Database($"Data Source={testDbPath}");
        db.Initialize();
        
        sessionRepository = new SessionRepository(db);
    }

    [Fact]
    public void CreateAndGetAllSessions_ShouldWorkCorrectly()
    {
        // Arrange
        var sessionsBefore = sessionRepository.GetAllSessions();
        
        // Act
        sessionRepository.CreateSession(FixedDate);
        var sessions = sessionRepository.GetAllSessions();
        
        // Assert
        Assert.Equal(sessionsBefore.Count + 1, sessions.Count);
        Assert.Contains(sessions, s => s.Day == FixedDate);
    }

    [Fact]
    public void UpdateSession_ShouldWorkCorrectly()
    {
        // Arrange
        var session = new Session {Id = 1, Day = FixedDate};
        
        // Act
        sessionRepository.UpdateSession(session);
        var updatedSession = sessionRepository.GetAllSessions().SingleOrDefault(s => s.Id == session.Id);
        
        // Assert
        Assert.Equal(session.Day, updatedSession?.Day);
    }

    [Fact]
    public void DeleteSession_ShouldWorkCorrectly()
    {
        // Arrange
        var sessionsBefore = sessionRepository.GetAllSessions();
        
        // Act
        sessionRepository.DeleteSession(sessionsBefore[0].Id);
        var sessionsAfter = sessionRepository.GetAllSessions();
        
        // Assert
        Assert.Equal(sessionsBefore.Count - 1, sessionsAfter.Count);
        Assert.DoesNotContain(sessionsAfter, s => s.Id == sessionsBefore[0].Id);
        Assert.DoesNotContain(sessionsAfter, s => s.Day == sessionsBefore[0].Day);
    }

    [Fact]
    public void Delete_NonExistentLog_ThrowsOrIgnoresGracefully()
    {
        // Arrange
        var session = new Session {Id = 420, Day = FixedDate};
        
        // Act & Assert
        Assert.Throws<CodingTrackerException>(() => sessionRepository.DeleteSession(session.Id));
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        SqliteConnection.ClearAllPools();
        if(File.Exists(testDbPath))
        {
            File.Delete(testDbPath);
        }
    }
}