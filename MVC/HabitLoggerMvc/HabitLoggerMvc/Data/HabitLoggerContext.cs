using System.Data;
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
}