namespace Freem.Entities.UseCases.Contracts.Filter;

public struct Offset
{
    private const int DefaultIntValue = 0;

    public static Offset DefaultValue { get; } = new Offset(DefaultIntValue);
    
    private readonly ulong _value;

    public Offset(int value = DefaultIntValue)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);

        _value = (ulong)value;
    }

    public static implicit operator int(Offset offset)
    {
        return (int)offset._value;
    }
}