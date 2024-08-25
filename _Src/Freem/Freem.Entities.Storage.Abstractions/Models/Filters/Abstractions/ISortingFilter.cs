using Freem.Sorting;

namespace Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;

public interface ISortingFilter<TSortField>
    where TSortField : struct, Enum
{
    SortOptions<TSortField> Sorting { get; }
}