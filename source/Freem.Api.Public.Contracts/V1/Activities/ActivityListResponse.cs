using Freem.Api.Public.Contracts.V1.Activities.Dtos;

namespace Freem.Api.Public.Contracts.V1.Activities;

/// <summary>Response containing a page of activities.</summary>
public sealed class ActivityListResponse
{
    /// <summary>Total number of activities matching the query, regardless of paging.</summary>
    public int TotalCount { get; }

    /// <summary>The requested page of activities.</summary>
    public IReadOnlyList<ActivityDto> Activities { get; }

    public ActivityListResponse(int totalCount, IReadOnlyList<ActivityDto> activities)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);
        ArgumentNullException.ThrowIfNull(activities);

        TotalCount = totalCount;
        Activities = activities;
    }
}
