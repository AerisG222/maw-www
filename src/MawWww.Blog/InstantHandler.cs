using System.Data;
using Dapper;
using NodaTime;

namespace MawWww.Blog;

// https://github.com/mattjohnsonpint/Dapper-NodaTime/blob/master/src/Dapper.NodaTime/InstantHandler.cs
class InstantHandler
    : SqlMapper.TypeHandler<Instant>
{
    public static readonly InstantHandler Default = new();

    private InstantHandler() { }

    public override void SetValue(IDbDataParameter parameter, Instant value)
    {
        parameter.Value = value;
    }

    // Npgsql already provides the correct typed value
    public override Instant Parse(object value)
    {
        return (Instant)value;
    }
}
