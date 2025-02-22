namespace Freem.Entities.Models.Users;

public sealed class DayUtcOffset
{
    private readonly TimeSpan _value;
    
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