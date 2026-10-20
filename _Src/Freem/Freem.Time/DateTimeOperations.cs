namespace Freem.Time;

public static class DateTimeOperations
{
    public static DateTimeOffset Min(DateTimeOffset left, DateTimeOffset right)
    {
        return left < right ? left : right;
    }
    
    public static DateTimeOffset Max(DateTimeOffset left, DateTimeOffset right)
    {
        return left > right ? left : right;
    }

    // Точность времени в системе — миллисекунды: этого хватает JS Date/ISO 8601 и без потерь помещается в timestamptz
    public static DateTimeOffset TruncateToMilliseconds(DateTimeOffset value)
    {
        var ticks = value.Ticks - value.Ticks % TimeSpan.TicksPerMillisecond;
        return new DateTimeOffset(ticks, value.Offset);
    }
}