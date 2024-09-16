namespace Freem.Entities.Storage.Abstractions.Models.Filters.Models;

public struct Limit
{
    public const int DefaultValue = 100;
    public const int MaxValue = 1000;
    
    public int Value { get; }

    public Limit(int value = DefaultValue)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, MaxValue, nameof(value));
        
        Value = value;
    }
}