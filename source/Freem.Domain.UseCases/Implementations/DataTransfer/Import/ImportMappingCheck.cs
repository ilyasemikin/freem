using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.DataTransfer.Import;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Import;

/// <summary>
/// Checks that import activities and tags referenced by the selected rows of an import are mapped.
/// </summary>
public sealed class ImportMappingCheck
{
    private readonly IReadOnlyList<ImportDocumentActivity> _activities;
    private readonly IReadOnlyList<ImportDocumentTag> _tags;
    private readonly HashSet<ImportActivityId> _activityIds = [];
    private readonly HashSet<ImportTagId> _tagIds = [];

    public ImportMappingCheck(
        IReadOnlyList<ImportDocumentActivity> activities,
        IReadOnlyList<ImportDocumentTag> tags)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(tags);

        _activities = activities;
        _tags = tags;
    }

    public void Add(ImportRow row)
    {
        ArgumentNullException.ThrowIfNull(row);

        _activityIds.UnionWith(row.Record.ActivityIds);
        _tagIds.UnionWith(row.Record.TagIds);
    }

    /// <summary>
    /// Ids of import activities used by the rows that are not mapped (or absent in the import).
    /// </summary>
    public IReadOnlyList<ImportActivityId> FindUnmappedActivities()
    {
        var mappedActivityIds = _activities
            .Where(a => a.MappedActivityId is not null)
            .Select(a => a.Activity.Id)
            .ToHashSet();
        return _activityIds
            .Where(id => !mappedActivityIds.Contains(id))
            .OrderBy(id => id.ToString(), StringComparer.Ordinal)
            .ToArray();
    }

    /// <summary>
    /// Ids of import tags used by the rows that are not mapped (or absent in the import).
    /// </summary>
    public IReadOnlyList<ImportTagId> FindUnmappedTags()
    {
        var mappedTagIds = _tags
            .Where(t => t.MappedTagId is not null)
            .Select(t => t.Tag.Id)
            .ToHashSet();
        return _tagIds
            .Where(id => !mappedTagIds.Contains(id))
            .OrderBy(id => id.ToString(), StringComparer.Ordinal)
            .ToArray();
    }
}
