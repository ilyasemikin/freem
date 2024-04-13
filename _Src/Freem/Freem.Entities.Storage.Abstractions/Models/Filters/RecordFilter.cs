using Freem.DateTimePeriods;
using Freem.Entities.Storage.Abstractions.Models.Filters.Base;
using Freem.Entities.Storage.Abstractions.Models.Sorting;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public class RecordFilter : BaseFilter<RecordSortField>
{
    public IReadOnlySet<string>? Ids { get; init; }
    public IReadOnlySet<string>? UserId { get; init; }
    public IReadOnlySet<string>? CategoryIds { get; init; }
    public IReadOnlySet<string>? TagIds { get; init; }
    public IReadOnlySet<string?>? Names { get; init; }
    public IReadOnlySet<string?>? Descriptions { get; init; }
    public DateTimePeriod? AllowedPeriod { get; init; }
}
