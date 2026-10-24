using Freem.Domain.Activities;

namespace Freem.Api.Public.Contracts.V1.Imports;

/// <summary>Request body for updating the mapping of an import activity to an existing activity of the user.</summary>
public sealed class ImportActivityMappingUpdateRequest
{
    /// <summary>Existing activity; null removes the mapping.</summary>
    public ActivityId? ActivityId { get; }

    public ImportActivityMappingUpdateRequest(ActivityId? activityId)
    {
        ActivityId = activityId;
    }
}
