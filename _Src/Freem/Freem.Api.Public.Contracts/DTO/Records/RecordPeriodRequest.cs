using Freem.Domain.Records.Models;
using Freem.Time.Models;

namespace Freem.Api.Public.Contracts.DTO.Records;

/// <summary>A period of a record: a time interval with an optional description.</summary>
public sealed class RecordPeriodRequest
{
    /// <summary>Start and end of the period. The duration must be positive.</summary>
    public DateTimePeriod Period { get; }

    /// <summary>Optional period description.</summary>
    public RecordPeriodDescription? Description { get; init; }

    public RecordPeriodRequest(DateTimePeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);

        Period = period;
    }
}
