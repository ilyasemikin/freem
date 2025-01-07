namespace Freem.Time;

public static class DateTimeOperations
{
    public static bool EqualsUpToSeconds(DateTimeOffset left, DateTimeOffset right)
    {
        return (left - right).TotalSeconds < 1;
    }
    
    public static bool EqualsUpToSeconds(DateTime left, DateTime right)
    {
        return (left - right).TotalSeconds < 1;
    }
}