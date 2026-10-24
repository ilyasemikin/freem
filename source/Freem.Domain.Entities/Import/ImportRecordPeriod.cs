using Freem.Domain.Records.Models;
using Freem.Domain.Users.Models;
using Freem.Time;

namespace Freem.Domain.Entities.Import;

public sealed class ImportRecordPeriod
{
    public DateTimePeriod Period { get; }
    public TimeZoneOffset TimeZoneOffset { get; }
    public RecordPeriodDescription? Description { get; }

    public ImportRecordPeriod(
        DateTimePeriod period,
        TimeZoneOffset timeZoneOffset,
        RecordPeriodDescription? description = null)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(timeZoneOffset);

        Period = period;
        TimeZoneOffset = timeZoneOffset;
        Description = description;
    }
}
