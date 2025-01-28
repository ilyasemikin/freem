namespace Freem.Time.Extensions;

public static class DateTimeExtensions
{
    public static DateOnly ToDateOnly(this DateTime value)
    {
        return new DateOnly(value.Year, value.Month, value.Day);
    }
}