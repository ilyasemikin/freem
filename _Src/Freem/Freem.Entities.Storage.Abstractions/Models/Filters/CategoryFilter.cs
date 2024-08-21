using Freem.Entities.Storage.Abstractions.Models.Filters.Base;
using Freem.Entities.Storage.Abstractions.Models.Sorting;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public class CategoryFilter : BaseFilter<CategorySortField>
{
    public IReadOnlySet<string>? Ids { get; init; }
    public IReadOnlySet<string>? UserIds { get; init; }
    public IReadOnlySet<string>? TagIds { get; init; }
    public IReadOnlySet<string?>? Names { get; init; }
    public IReadOnlySet<CategoryStatus>? CategoryStatuses { get; init; }
}
