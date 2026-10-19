using Freem.Sorting;

namespace Freem.Domain.Storage.Models.Filters.Abstractions;

public interface ISortingFilter<TSortField> : IFilter
    where TSortField : struct, Enum
{
    SortOptions<TSortField> Sorting { get; }
}