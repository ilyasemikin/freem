using Freem.Domain.Entities.Activities;
using Freem.Domain.UseCases.Contracts.Filter;

namespace Freem.Domain.UseCases.Contracts.Activities.Search;

public sealed class SearchActivityByNameResponse
{
    public IReadOnlyList<Activity> Activities { get; }
    public TotalCount TotalCount { get; }

    public SearchActivityByNameResponse(IReadOnlyList<Activity> activities, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(totalCount);

        Activities = activities;
        TotalCount = totalCount;
    }
}