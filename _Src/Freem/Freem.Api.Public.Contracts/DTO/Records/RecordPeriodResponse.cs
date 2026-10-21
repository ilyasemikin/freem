using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Users.Models;
using Freem.Time.Models;

namespace Freem.Api.Public.Contracts.DTO.Records;

/// <summary>A period of a record.</summary>
public sealed class RecordPeriodResponse
{
    /// <summary>Period id.</summary>
    public RecordPeriodId Id { get; }

    /// <summary>Start and end of the period.</summary>
    public DateTimePeriod Period { get; }

    /// <summary>Time zone offset of the user when the period was created or its time last changed; informational only.</summary>
    public TimeZoneOffset TimeZoneOffset { get; }

    /// <summary>Period description, if set.</summary>
    public RecordPeriodDescription? Description { get; init; }

    public RecordPeriodResponse(RecordPeriodId id, DateTimePeriod period, TimeZoneOffset timeZoneOffset)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(timeZoneOffset);

        Id = id;
        Period = period;
        TimeZoneOffset = timeZoneOffset;
    }
}
