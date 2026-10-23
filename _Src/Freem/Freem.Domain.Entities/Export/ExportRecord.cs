using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Tags;

namespace Freem.Domain.Entities.Export;

public sealed class ExportRecord : ExportItem
{
    public RecordId Id { get; }
    public RecordName? Name { get; }
    public RecordDescription? Description { get; }
    public IReadOnlyList<ActivityId> ActivityIds { get; }
    public IReadOnlyList<TagId> TagIds { get; }
    public IReadOnlyList<RecordPeriodSnapshot> Periods { get; }

    public ExportRecord(
        RecordId id,
        RecordName? name,
        RecordDescription? description,
        IReadOnlyList<ActivityId> activityIds,
        IReadOnlyList<TagId> tagIds,
        IReadOnlyList<RecordPeriodSnapshot> periods)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(activityIds);
        ArgumentNullException.ThrowIfNull(tagIds);
        ArgumentNullException.ThrowIfNull(periods);

        Id = id;
        Name = name;
        Description = description;
        ActivityIds = activityIds;
        TagIds = tagIds;
        Periods = periods;
    }
}
