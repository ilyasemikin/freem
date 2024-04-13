using Freem.Entities.Storage.Abstractions.Models.Filters.Base;
using Freem.Entities.Storage.Abstractions.Models.Sorting;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public class TagFilter : BaseFilter<TagSortField>
{
    public IReadOnlySet<string>? Ids { get; init; }
    public IReadOnlySet<string>? Name { get; init; }
}
