namespace Freem.Entities.Storage.Abstractions.Models.Filters.Models;

public struct Offset
{
    public const int DefaultValue = 0;
    
    public int Value { get; }

    public static Offset Default { get; } = new(0);

    public Offset(int value = DefaultValue)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        
        Value = value;
    }
}