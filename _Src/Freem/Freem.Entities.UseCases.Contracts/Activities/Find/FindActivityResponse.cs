using Freem.Domain.Entities.Activities;
using Freem.Entities.UseCases.Contracts.Filter;

namespace Freem.Entities.UseCases.Contracts.Activities.Find;

public sealed class FindActivityResponse
{
    public IReadOnlyList<Activity> Activities { get; }
    public TotalCount TotalCount { get; }

    public FindActivityResponse(IReadOnlyList<Activity> activities, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(totalCount);

        Activities = activities;
        TotalCount = totalCount;
    }
}