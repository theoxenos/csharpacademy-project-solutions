namespace CodingTracker.Repositories;

public class SessionLogRepository
{
    private readonly Database db;

    public SessionLogRepository()
    {
        db = new();
    }
    
    public SessionLogRepository(Database database)
    {
        db = database;
    }

    public IEnumerable<SessionLog> GetLogsBySessionId(int sessionId)
    {
        using var connection = db.GetConnection();
        return connection
            .Query<SessionLog>("SELECT * FROM Logs WHERE SessionId = @SessionId", new { SessionId = sessionId });
    }

    public void DeleteSessionLog(SessionLog log)
    {
        using var connection = db.GetConnection();
        var rows = connection.Execute("DELETE FROM Logs WHERE Id = @Id", log);
        if (rows == 0)
        {
            throw new InvalidOperationException("Session log with specified ID not found.");
        }
    }

    public void UpdateSessionLog(SessionLog sessionLog)
    {
        if (sessionLog.EndTime < sessionLog.StartTime)
            throw new ArgumentException("Log not created due to: End Time must be greater than Start Time.",
                nameof(sessionLog));

        using var connection = db.GetConnection();
        connection.Execute(
            "UPDATE Logs SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id", sessionLog);
    }

    public List<SessionLog> GetAllSessionLogs()
    {
        try
        {
            using var connection = db.GetConnection();
            var logRecords = connection.Query<SessionLog>("SELECT * FROM Logs");

            // var logs = logRecords.Select(l => new SessionLog
            // {
            //     Id = (int)l.Id,
            //     SessionId = (int)l.SessionId,
            //     StartTime = TimeOnly.Parse(l.StartTime),
            //     EndTime = TimeOnly.Parse(l.EndTime),
            //     Duration = TimeSpan.FromTicks(l.Duration)
            // });

            return logRecords.ToList();
        }
        catch (Exception e)
        {
            throw new CodingTrackerException("An unexpected error occurred while getting the session logs.", e);
        }
    }

    public void CreateLog(SessionLog sessionLog)
    {
        if (sessionLog.EndTime < sessionLog.StartTime)
            throw new ArgumentException("Log not created due to: End Time must be greater than Start Time.",
                nameof(sessionLog));

        try
        {
            using var connection = db.GetConnection();

            var query =
                "INSERT INTO Logs (SessionId, StartTime, EndTime, Duration) VALUES (@SessionId, @StartTime, @EndTime, @Duration)";
            connection.Execute(query, sessionLog);
        }
        catch (Exception e)
        {
            throw new CodingTrackerException("An unexpected error occurred while creating the log.", e);
        }
    }
}