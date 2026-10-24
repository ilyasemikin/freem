using Freem.Domain.Records.Models;
using Freem.Domain.Users.Models;
using Freem.Time;

namespace Freem.Api.Public.Contracts.V1.Imports.Dtos;

/// <summary>A period of an import row.</summary>
public sealed class ImportRowPeriodDto
{
    /// <summary>Start and end of the period.</summary>
    public DateTimePeriod Period { get; }

    /// <summary>Time zone offset of the period.</summary>
    public TimeZoneOffset TimeZoneOffset { get; }

    /// <summary>Period description, if set.</summary>
    public RecordPeriodDescription? Description { get; init; }

    public ImportRowPeriodDto(DateTimePeriod period, TimeZoneOffset timeZoneOffset)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(timeZoneOffset);

        Period = period;
        TimeZoneOffset = timeZoneOffset;
    }
}
