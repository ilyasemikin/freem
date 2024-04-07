namespace Freem.DateTimePeriods.Helpers;

internal static class DateTimeOffsetComparer
{
    public static DateTimeOffset Max(DateTimeOffset left, DateTimeOffset right)
    {
        return left > right ? left : right;
    }

    public static DateTimeOffset Min(DateTimeOffset left, DateTimeOffset right)
    {
        return left < right ? left : right;
    }
}
