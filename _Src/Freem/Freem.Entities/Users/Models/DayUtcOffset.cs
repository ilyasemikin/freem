namespace Freem.Entities.Users.Models;

public sealed class DayUtcOffset
{
    private readonly TimeSpan _value;
    
    public static DayUtcOffset Default { get; } = new(TimeSpan.Zero);
    
    public long Ticks => _value.Ticks;
    
    public DayUtcOffset(TimeSpan value)
    {
        _value = value;
    }

    public static implicit operator TimeSpan(DayUtcOffset dayUtcOffset)
    {
        return dayUtcOffset._value;
    }

    public static implicit operator DayUtcOffset(TimeSpan value)
    {
        return new DayUtcOffset(value);
    }
}