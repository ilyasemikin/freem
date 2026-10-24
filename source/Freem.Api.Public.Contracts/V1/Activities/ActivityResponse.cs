using Freem.Api.Public.Contracts.V1.Activities.Dtos;

namespace Freem.Api.Public.Contracts.V1.Activities;

/// <summary>Response containing an activity.</summary>
public sealed class ActivityResponse
{
    /// <summary>The activity.</summary>
    public ActivityDto Activity { get; }

    public ActivityResponse(ActivityDto activity)
    {
        ArgumentNullException.ThrowIfNull(activity);

        Activity = activity;
    }
}
