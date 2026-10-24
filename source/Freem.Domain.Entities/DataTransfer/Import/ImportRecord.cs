using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Records.Models;

namespace Freem.Domain.Entities.DataTransfer.Import;

public sealed class ImportRecord
{
    public ImportRecordId Id { get; }
    public RecordName? Name { get; }
    public RecordDescription? Description { get; }
    public IReadOnlyList<ImportActivityId> ActivityIds { get; }
    public IReadOnlyList<ImportTagId> TagIds { get; }
    public IReadOnlyList<ImportRecordPeriod> Periods { get; }

    public ImportRecord(
        ImportRecordId id,
        RecordName? name,
        RecordDescription? description,
        IReadOnlyList<ImportActivityId> activityIds,
        IReadOnlyList<ImportTagId> tagIds,
        IReadOnlyList<ImportRecordPeriod> periods)
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
