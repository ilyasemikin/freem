namespace Freem.Time.Extensions;

public static class DateTimeOffsetExtensions
{
    public static DateOnly ToDateOnly(this DateTimeOffset value)
    {
        return new DateOnly(value.Year, value.Month, value.Day);
    }
}