using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Import;

namespace Freem.Api.Public.Contracts.V1.Imports.Dtos;

/// <summary>An import activity: an activity found in the imported file.</summary>
public sealed class ImportActivityDto
{
    /// <summary>Key of the activity in the imported file.</summary>
    public ImportActivityId Id { get; }

    /// <summary>Activity name.</summary>
    public ActivityName Name { get; }

    /// <summary>Activity color, if the imported file has one.</summary>
    public ActivityColor? Color { get; init; }

    /// <summary>Activity status in the imported file.</summary>
    public ActivityStatus Status { get; }

    /// <summary>Keys of the import tags related to the activity.</summary>
    public IReadOnlyList<ImportTagId> TagIds { get; }

    /// <summary>Existing Freem activity the import activity is mapped to, absent while not mapped.</summary>
    public ActivityId? MappedActivityId { get; init; }

    public ImportActivityDto(
        ImportActivityId id,
        ActivityName name,
        ActivityStatus status,
        IReadOnlyList<ImportTagId> tagIds)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(tagIds);

        Id = id;
        Name = name;
        Status = status;
        TagIds = tagIds;
    }
}
