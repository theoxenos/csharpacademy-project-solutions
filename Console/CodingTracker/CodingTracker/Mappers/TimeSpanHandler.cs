using System.Data;

namespace CodingTracker.Mappers;

public class TimeSpanHandler: SqlMapper.TypeHandler<TimeSpan>
{
    public override void SetValue(IDbDataParameter parameter, TimeSpan value)
    {
        parameter.Value = value.Ticks;
        parameter.DbType = DbType.Int64;
    }

    public override TimeSpan Parse(object value)
    {
        if (value is long ticks)
        {
            return TimeSpan.FromTicks(ticks);
        }

        if (long.TryParse(value.ToString(), out long result))
        {
            return TimeSpan.FromTicks(result);
        }

        return TimeSpan.Parse(value.ToString()!);
    }
}