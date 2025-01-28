namespace Freem.Entities.Storage.Abstractions.Models.Filters.Models;

public class Offset
{
    public const int DefaultValue = 0;
    
    public int Value { get; }

    public Offset(int value = DefaultValue)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        
        Value = value;
    }

    public static implicit operator Offset(int value)
    {
        return new Offset(value);
    }
}