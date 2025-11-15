using System.Data;
using Dapper;

namespace ShoppingList.Server.Infrastructure;

public static class DapperSetup
{
    private static bool _configured;

    public static void Register()
    {
        if (_configured) return;
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        _configured = true;
    }

    private sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.Value = value.ToString("yyyy-MM-dd");
        }

        public override DateOnly Parse(object value)
        {
            return value switch
            {
                string s => DateOnly.Parse(s),
                DateTime dt => DateOnly.FromDateTime(dt),
                _ => DateOnly.Parse(value.ToString() ?? string.Empty)
            };
        }
    }
}
