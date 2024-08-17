namespace Freem.Sorting;

public sealed record SortOption<TSortField>(TSortField Field, SortOrder Order)
    where TSortField : Enum
{
    public Type SortFieldType => typeof(TSortField);
}
