using Freem.Time.Models;

namespace Freem.Domain.Records.Models;

public sealed class RecordPeriodSnapshot
{
    public RecordPeriodId Id { get; }
    public DateTimePeriod Period { get; }
    public RecordPeriodDescription? Description { get; }

    public RecordPeriodSnapshot(
        RecordPeriodId id,
        DateTimePeriod period,
        RecordPeriodDescription? description = null)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(period);

        Id = id;
        Period = period;
        Description = description;
    }
}
