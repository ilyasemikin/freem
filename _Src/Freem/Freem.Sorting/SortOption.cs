using Freem.Enums.Exceptions;

namespace Freem.Sorting;

public sealed class SortOption<TSortField>
    where TSortField : struct, Enum
{
    public TSortField Field { get; }
    public SortOrder Order { get; }
    
    public SortOption(TSortField field, SortOrder order)
    {
        InvalidEnumValueException<TSortField>.ThrowIfValueInvalid(field);
        InvalidEnumValueException<SortOrder>.ThrowIfValueInvalid(order);

        Field = field;
        Order = order;
    }
}
