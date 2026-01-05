using CodingTracker.Mappers;

namespace CodingTracker.Data;

public class Database
{
    private readonly Configuration? appConfiguration;
    private readonly string connectionString;
    private readonly string? databasePath;

    public Session[] SeedSessions { get; } = new[]
    {
        new Session { Day = DateOnly.Parse("1-1-24") },
        new Session { Day = DateOnly.Parse("2-1-24") },
        new Session { Day = DateOnly.Parse("10-2-24") },
        new Session { Day = DateOnly.Parse("4-1-26") },
        new Session { Day = DateOnly.Parse("5-1-26") }
    };

    public SessionLog[] SeedLogs { get; } = new[]
    {
        new SessionLog
        {
            Id = 1,
            SessionId = 1,
            StartTime = TimeOnly.Parse("09:00"),
            EndTime = TimeOnly.Parse("10:00"),
            Duration = TimeSpan.FromTicks(36000000000L)
        },
        new SessionLog
        {
            Id = 2,
            SessionId = 1,
            StartTime = TimeOnly.Parse("10:00"),
            EndTime = TimeOnly.Parse("11:00"),
            Duration = TimeSpan.FromTicks(36000000000L)
        },
        new SessionLog
        {
            Id = 3,
            SessionId = 2,
            StartTime = TimeOnly.Parse("11:00"),
            EndTime = TimeOnly.Parse("12:00"),
            Duration = TimeSpan.FromTicks(36000000000L)
        },
        new SessionLog
        {
            Id = 4,
            SessionId = 2,
            StartTime = TimeOnly.Parse("12:00"),
            EndTime = TimeOnly.Parse("13:00"),
            Duration = TimeSpan.FromTicks(36000000000L)
        },
        new SessionLog
        {
            Id = 5,
            SessionId = 3,
            StartTime = TimeOnly.Parse("13:00"),
            EndTime = TimeOnly.Parse("14:00"),
            Duration = TimeSpan.FromTicks(36000000000L)
        },
        new SessionLog
        {
            Id = 6,
            SessionId = 3,
            StartTime = TimeOnly.Parse("15:00"),
            EndTime = TimeOnly.Parse("16:55"),
            Duration = TimeSpan.FromTicks(69000000000L)
        }
    };

    public Database()
    {
        appConfiguration = new Configuration();
        databasePath = appConfiguration.GetConfigurationItemByKey("DatabasePath");
        var configConnectionString = appConfiguration.GetConfigurationItemByKey("ConnectionString");
        connectionString = configConnectionString + databasePath;
    }

    public Database(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void Initialize()
    {
        SetMappers();
        CreateTables();
        SeedDatabase();
    }

    public SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(connectionString);

        try
        {
            connection.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine($"A problem occured during database opening: {e.Message}");
        }

        return connection;
    }

    private void CreateTables()
    {
        using var connection = GetConnection();

        var createTableQuery =
            """
                PRAGMA foreign_keys = ON;
                CREATE TABLE IF NOT EXISTS Sessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Day TEXT NOT NULL UNIQUE
                );

                CREATE TABLE IF NOT EXISTS Logs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    SessionId INTEGER NOT NULL,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration INTEGER,
                    FOREIGN KEY (SessionId) REFERENCES Sessions(Id) ON DELETE CASCADE
                );
            """;
        connection.Execute(createTableQuery);
    }

    private void InsertSessions()
    {
        using var connection = GetConnection();

        var existingData = connection.QuerySingle<int>("SELECT COUNT(*) FROM Sessions");
        if (existingData > 0)
        {
            return;
        }

        var sessionsSeedSql = "INSERT INTO Sessions (Day) VALUES (@Day)";
        connection.Execute(sessionsSeedSql, SeedSessions);
    }

    private void InsertLogs()
    {
        using var connection = GetConnection();

        var existingData = connection.QuerySingle<int>("SELECT COUNT(*) FROM Logs");
        if (existingData > 0)
        {
            return;
        }

        var logsSeedSql = """
                          INSERT INTO Logs (SessionId, StartTime, EndTime, Duration) 
                          VALUES (@SessionId, @StartTime, @EndTime, @Duration)
                          """;
        connection.Execute(logsSeedSql, SeedLogs);
    }

    private void SetMappers()
    {
        SqlMapper.AddTypeHandler(new DateOnlyHandler());
        SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        SqlMapper.AddTypeHandler(new TimeOnlyHandler());
        SqlMapper.AddTypeHandler(new TimeSpanHandler());
    }

    private void SeedDatabase()
    {
        InsertSessions();
        InsertLogs();
    }
}