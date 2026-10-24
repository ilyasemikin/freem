using Freem.Domain.Users.Models;
using Freem.Time;

namespace Freem.Domain.Records.Models;

public sealed class RecordPeriodSnapshot
{
    public RecordPeriodId Id { get; }
    public DateTimePeriod Period { get; }
    public TimeZoneOffset TimeZoneOffset { get; }
    public RecordPeriodDescription? Description { get; }

    public RecordPeriodSnapshot(
        RecordPeriodId id,
        DateTimePeriod period,
        TimeZoneOffset timeZoneOffset,
        RecordPeriodDescription? description = null)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(timeZoneOffset);

        Id = id;
        Period = period;
        TimeZoneOffset = timeZoneOffset;
        Description = description;
    }
}
