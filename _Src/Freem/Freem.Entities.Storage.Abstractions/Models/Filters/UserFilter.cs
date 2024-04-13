using Freem.Entities.Storage.Abstractions.Models.Filters.Base;
using Freem.Entities.Storage.Abstractions.Models.Sorting;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public class UserFilter : BaseFilter<UserSortField>
{
    public IReadOnlySet<string>? Ids { get; init; }
}
