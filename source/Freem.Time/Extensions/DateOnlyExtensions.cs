namespace Freem.Time.Extensions;

public static class DateOnlyExtensions
{
    public static DateTime ToUtcDateTime(this DateOnly date, TimeOnly time)
    {
        return date.ToDateTime(time, DateTimeKind.Utc);
    }

    public static DateTime ToUtcDateTime(this DateOnly date)
    {
        var time = TimeOnly.MinValue;
        return date.ToUtcDateTime(time);
    }
    
    public static DateTime ToUtcDateTime(this DateOnly date, int hour)
    {
        var time = new TimeOnly(hour, 0);
        return date.ToUtcDateTime(time);
    }
}