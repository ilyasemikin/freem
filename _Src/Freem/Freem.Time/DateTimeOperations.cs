using Freem.Time.Models;

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
    
    public static bool EqualsUpToSeconds(DateTimeOffset left, DateTimeOffset right)
    {
        return (left - right).TotalSeconds < 1;
    }
    
    public static bool EqualsUpToSeconds(DateTime left, DateTime right)
    {
        return (left - right).TotalSeconds < 1;
    }
}