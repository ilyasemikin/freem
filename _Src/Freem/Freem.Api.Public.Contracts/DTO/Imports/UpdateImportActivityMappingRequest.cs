using Freem.Domain.Activities;

namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>Request body for updating the mapping of an import activity to an existing activity of the user.</summary>
public sealed class UpdateImportActivityMappingRequest
{
    /// <summary>Existing activity; null removes the mapping.</summary>
    public ActivityId? ActivityId { get; }

    public UpdateImportActivityMappingRequest(ActivityId? activityId)
    {
        ActivityId = activityId;
    }
}
