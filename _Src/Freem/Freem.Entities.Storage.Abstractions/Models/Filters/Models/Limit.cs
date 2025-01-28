namespace Freem.Entities.Storage.Abstractions.Models.Filters.Models;

public class Limit
{
    private const int DefaultValue = 100;
    
    public const int MaxValue = 1000;

    public int Value { get; }
    
    public static Limit Default { get; } = new();
    
    public Limit(int value = DefaultValue)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, MaxValue, nameof(value));
        
        Value = value;
    }

    public static implicit operator Limit(int value)
    {
        return new Limit(value);
    }
}