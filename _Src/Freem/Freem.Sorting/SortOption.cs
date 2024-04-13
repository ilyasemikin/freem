namespace Freem.Sorting;

public record SortOption<TSortField>(TSortField Field, SortOrder Order)
    where TSortField : Enum
{
    public Type SortFieldType => typeof(TSortField);
}
