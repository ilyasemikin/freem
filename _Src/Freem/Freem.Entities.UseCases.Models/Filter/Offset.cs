using StorageOffset = Freem.Entities.Storage.Abstractions.Models.Filters.Models.Offset;

namespace Freem.Entities.UseCases.Models.Filter;

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

    public static implicit operator StorageOffset(Offset value)
    {
        return new StorageOffset((int)value._value);
    }
}