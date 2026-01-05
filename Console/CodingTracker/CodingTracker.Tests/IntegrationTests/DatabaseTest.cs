using CodingTracker.Data;
using CodingTracker.Repositories;
using CodingTracker.Validators;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Tests.IntegrationTests;

public class DatabaseTest: IDisposable
{
    private readonly Database database;
    private readonly SessionRepository sessionRepository;
    private const string testDbPath = "test.db";
    
    public DatabaseTest()
    {
        if(File.Exists(testDbPath))
        {
            File.Delete(testDbPath);
        }
        database = new Database($"Data Source={testDbPath}");
        sessionRepository = new SessionRepository(database);
    }
    
    [Fact]
    public void Initialize_ShouldSeedInitialData()
    {
        // Act
        database.Initialize();
        var sessions = sessionRepository.GetAllSessions();

        // Assert
        Assert.Equal(database.SeedSessions.Length, sessions.Count); // We expect the 3 sessions defined in SeedDatabase
        Assert.Contains(sessions, s => s.Day.ToString(Validator.DateFormat) == "1-1-24");
        Assert.Contains(sessions, s => s.Id == 1);
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