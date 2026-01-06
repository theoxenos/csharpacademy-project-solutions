using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace HabitLoggerMvc.Data;

public class HabitLoggerContext
{
    private readonly string _connectionString;
    private string _databaseName = "habits_db";

    public HabitLoggerContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                            throw new ArgumentNullException(nameof(configuration),
                                "Connection string 'DefaultConnection' is required.");

        var connectionStringBuilder = new SqlConnectionStringBuilder(_connectionString);
        if (!string.IsNullOrEmpty(connectionStringBuilder.InitialCatalog))
        {
            _databaseName = connectionStringBuilder.InitialCatalog;
        }
    }

    public IDbConnection GetConnection()
    {
        var connection = new SqlConnection(_connectionString);

        return connection;
    }

    public async Task Init()
    {
        await CreateDatabase();
        await CreateTables();
        await SeedData();
    }

    private IDbConnection GetMasterConnection()
    {
        // Build a temporary connection string pointing to 'master'
        var builder = new SqlConnectionStringBuilder(_connectionString)
        {
            InitialCatalog = "master"
        };
        return new SqlConnection(builder.ToString());
    }

    private async Task CreateDatabase()
    {
        try
        {
            using var tempConnection = GetMasterConnection();
            var sql = $"""
                       if not exists(select * from sys.databases where name = '{_databaseName}')
                        create database [{_databaseName}]
                       """;
            await tempConnection.ExecuteAsync(sql);
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred during database creation: {e.Message}");
        }
    }

    private async Task CreateTables()
    {
        const string sql = """
                           -- Habit units table
                           if not exists (select * from sys.tables where name = 'HabitUnits')
                               create table HabitUnits (
                                   Id int identity constraint PK_HabitUnits primary key,
                                   Name nvarchar(255) not null,
                                   constraint UQ_HabitUnitName unique (Name)
                               );

                           -- Habits table
                           if not exists (select * from sys.tables where name = 'Habits')
                               create table Habits (
                                   Id int identity constraint PK_Habits primary key,
                                   Name nvarchar(255) not null,
                                   HabitUnitId int not null,
                                   constraint UQ_HabitName unique (Name),
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
                                   constraint UQ_HabitIdLogDate unique (HabitId, Date),
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
        using var connection = GetConnection();
        await connection.ExecuteAsync(sql);
    }

    private async Task SeedData()
    {
        using var connection = GetConnection();
        var versionExists = connection.QueryFirstOrDefault<int?>("SELECT TOP 1 Id FROM DataVersion ORDER BY Id DESC");

        if (versionExists is not null)
            return;
        
        var seedData = new SeedData();

        const string habitUnitsSql = """
                                     SET IDENTITY_INSERT HabitUnits ON;
                                     INSERT INTO HabitUnits (Id, Name) VALUES ( @Id, @Name );
                                     SET IDENTITY_INSERT HabitUnits OFF;
                                     """;
        await connection.ExecuteAsync(habitUnitsSql, seedData.HabitUnitSeeds);

        const string habitsSql = """
                                 SET IDENTITY_INSERT Habits ON;
                                 INSERT INTO Habits (Id, Name, HabitUnitId) VALUES ( @Id, @Name, @HabitUnitId );
                                 SET IDENTITY_INSERT Habits OFF;
                                 """;
        await connection.ExecuteAsync(habitsSql, seedData.HabitSeeds);

        const string habitLogsSql =
            "INSERT INTO HabitLogs (HabitId, Date, Quantity) VALUES ( @HabitId, @Date, @Quantity )";
        await connection.ExecuteAsync(habitLogsSql, seedData.HabitLogs);

        const string dataVersionSql = "INSERT INTO DataVersion (Description, AppliedOn) VALUES ( @Description, @AppliedOn )";
        await connection.ExecuteAsync(dataVersionSql, new { Description = "Initial seed", AppliedOn = DateTime.Now });
    }
}