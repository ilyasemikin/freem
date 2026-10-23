using Freem.Domain.Records.Models;
using Freem.Domain.Users.Models;
using Freem.Time.Models;

namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>A period of an import row.</summary>
public sealed class ImportRowPeriodResponse
{
    /// <summary>Start and end of the period.</summary>
    public DateTimePeriod Period { get; }

    /// <summary>Time zone offset of the period.</summary>
    public TimeZoneOffset TimeZoneOffset { get; }

    /// <summary>Period description, if set.</summary>
    public RecordPeriodDescription? Description { get; init; }

    public ImportRowPeriodResponse(DateTimePeriod period, TimeZoneOffset timeZoneOffset)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(timeZoneOffset);

        Period = period;
        TimeZoneOffset = timeZoneOffset;
    }
}
