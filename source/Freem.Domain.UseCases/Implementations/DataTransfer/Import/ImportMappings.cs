using Freem.Domain.Activities;
using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Tags;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Import;

/// <summary>
/// Mappings of import activities and tags to Freem entities.
/// </summary>
public sealed class ImportMappings
{
    private readonly Dictionary<ImportActivityId, ActivityId> _activities = [];
    private readonly Dictionary<ImportTagId, TagId> _tags = [];

    public ImportMappings(
        IReadOnlyList<ImportDocumentActivity> activities,
        IReadOnlyList<ImportDocumentTag> tags)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(tags);

        foreach (var activity in activities)
        {
            if (activity.MappedActivityId is not null)
                _activities[activity.Activity.Id] = activity.MappedActivityId;
        }

        foreach (var tag in tags)
        {
            if (tag.MappedTagId is not null)
                _tags[tag.Tag.Id] = tag.MappedTagId;
        }
    }

    public ActivityId GetActivity(ImportActivityId importActivityId)
    {
        ArgumentNullException.ThrowIfNull(importActivityId);

        return _activities.TryGetValue(importActivityId, out var id)
            ? id
            : throw new InvalidOperationException($"Import activity [{importActivityId}] is not mapped");
    }

    public TagId GetTag(ImportTagId importTagId)
    {
        ArgumentNullException.ThrowIfNull(importTagId);

        return _tags.TryGetValue(importTagId, out var id)
            ? id
            : throw new InvalidOperationException($"Import tag [{importTagId}] is not mapped");
    }
}
