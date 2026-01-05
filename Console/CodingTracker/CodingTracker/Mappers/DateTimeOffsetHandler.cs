using System.Data;

namespace CodingTracker.Mappers;

public class DateTimeOffsetHandler : SqlMapper.TypeHandler<DateTime>
{
    // How to write to the database
    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        parameter.Value = value.ToString("yyyy-MM-dd HH:mm:ss");
    }

    // How to read from the database
    public override DateTime Parse(object value)
    {
        return DateTime.Parse((string)value);
    }
}