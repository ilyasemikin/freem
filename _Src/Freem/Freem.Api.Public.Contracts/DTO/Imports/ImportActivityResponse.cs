using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Import;

namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>An activity found in the imported file.</summary>
public sealed class ImportActivityResponse
{
    /// <summary>Key of the activity in the source file.</summary>
    public ImportActivityId SourceId { get; }

    /// <summary>Activity name.</summary>
    public ActivityName Name { get; }

    /// <summary>Activity color, if the source has one.</summary>
    public ActivityColor? Color { get; init; }

    /// <summary>Activity status in the source.</summary>
    public ActivityStatus Status { get; }

    /// <summary>Keys of the tags related to the activity in the source file.</summary>
    public IReadOnlyList<ImportTagId> TagSourceIds { get; }

    /// <summary>Decision on the matching Freem activity, absent while not made.</summary>
    public ImportResolutionResponse<ActivityId>? Resolution { get; init; }

    public ImportActivityResponse(
        ImportActivityId sourceId,
        ActivityName name,
        ActivityStatus status,
        IReadOnlyList<ImportTagId> tagSourceIds)
    {
        ArgumentNullException.ThrowIfNull(sourceId);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(tagSourceIds);

        SourceId = sourceId;
        Name = name;
        Status = status;
        TagSourceIds = tagSourceIds;
    }
}
