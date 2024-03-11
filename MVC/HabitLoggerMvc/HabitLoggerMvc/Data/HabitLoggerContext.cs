using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace HabitLoggerMvc.Data;

public class HabitLoggerContext(IConfiguration configuration)
{
    public async Task<IDbConnection> GetConnection()
    {
        var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        if (connection.State != ConnectionState.Open) await connection.OpenAsync();

        return connection;
    }

    public async Task Init()
    {
        // TODO Init DB?
        await CreateTables();
        await SeedData();
    }

    private async Task CreateTables()
    {
        var sql = """
                  -- Habit units table
                  if not exists (select * from sys.tables where name = 'HabitUnits')
                      create table HabitUnits (
                          Id int identity constraint PK_HabitUnits primary key,
                          Name nvarchar(255) not null
                      );
                  
                  -- Habits table
                  if not exists (select * from sys.tables where name = 'Habits')
                      create table Habits (
                          Id int identity constraint PK_Habits primary key,
                          Name nvarchar(255) not null,
                          HabitUnitId int not null,
                          foreign key (HabitUnitId) references HabitUnits(Id)
                              on delete NO ACTION
                      );
                  
                  -- Habit Logs table
                  if not exists (select * from sys.tables where name = 'HabitLogs')
                      create table HabitLogs (
                          Id int identity constraint PK_HabitLogs primary key,
                          HabitId int,
                          Date date,
                          Quantity int,
                          foreign key (HabitId) references Habits(Id)
                              on delete cascade
                      );
                      
                  -- Data Version table
                    if not exists (select * from sys.tables where name = 'DataVersion')
                      create table DataVersion (
                          Id int identity constraint PK_DataVersion primary key,
                          Description nvarchar(255),
                          AppliedOn datetime
                      );
                  """;
        using var connection = await GetConnection();
        await connection.ExecuteAsync(sql);
    }

    private async Task SeedData()
    {
        using var connection = await GetConnection();
        var versionExists = connection.QueryFirstOrDefault<int?>("SELECT TOP 1 Id FROM DataVersion ORDER BY Id DESC");

        if (versionExists is not null)
            return;

        var sql = """
                  -- Inserting into HabitUnits table
                  INSERT INTO HabitUnits (Name) VALUES
                  ('Medium glass'),
                  ('Small glass'),
                  ('Meters'),
                  ('Minutes'),
                  ('Pages');

                  -- Inserting into Habits table
                  INSERT INTO Habits (Name, HabitUnitId) VALUES
                  ('Drinking water', 1),
                  ('Drinking fruit sap', 2),
                  ('Walking', 3),
                  ('Meditation', 4),
                  ('Reading', 5);

                  -- Inserting into HabitLogs table
                  INSERT INTO HabitLogs (HabitId, Date, Quantity) VALUES
                  (1, '2023-01-01', 8),
                  (2, '2023-01-02', 5),
                  (3, '2023-01-03', 3),
                  (1, '2023-01-04', 7),
                  (2, '2023-01-05', 4),
                  (4, '2023-01-06', 30),
                  (5, '2023-01-07', 150);

                  -- Set flag in version table
                  INSERT INTO DataVersion (Description, AppliedOn) VALUES
                  ('Initial seed', @Today);
                  """;

        await connection.ExecuteAsync(sql, new { DateTime.Today });
    }
}