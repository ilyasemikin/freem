namespace Freem.Entities.UseCases.Models.Filter;

public sealed class TotalCount
{
    private readonly ulong _value;
    
    public TotalCount(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);

        _value = (ulong)value;
    }

    public static implicit operator TotalCount(int value)
    {
        return new TotalCount(value);
    }

    public static implicit operator int(TotalCount value)
    {
        return (int)value._value;
    }
}