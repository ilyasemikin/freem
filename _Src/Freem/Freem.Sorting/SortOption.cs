namespace Freem.Sorting;

public sealed class SortOption<TSortField>
    where TSortField : struct, Enum
{
    public TSortField Field { get; }
    public SortOrder Order { get; }
    
    public SortOption(TSortField field, SortOrder order)
    {
        if (!Enum.IsDefined(field))
            throw new ArgumentOutOfRangeException(nameof(field), field, null);

        if (!Enum.IsDefined(order))
            throw new ArgumentOutOfRangeException(nameof(order), order, null);

        Field = field;
        Order = order;
    }
}
