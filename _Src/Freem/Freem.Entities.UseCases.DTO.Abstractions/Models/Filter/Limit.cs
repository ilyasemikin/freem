namespace Freem.Entities.UseCases.DTO.Abstractions.Models.Filter;

public struct Limit
{
    private const int DefaultIntValue = 100;
    private const int MaxIntValue = 1000;
    
    public static Limit DefaultValue { get; } = new(DefaultIntValue);
    public static Limit MaxValue { get; } = new(MaxIntValue);
    
    private readonly int _value;

    public Limit(int value = DefaultIntValue)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, MaxIntValue);
        
        _value = value;
    }

    public static implicit operator int(Limit limit)
    {
        return limit._value;
    }
}