using Freem.Domain.Records.Models;
using Freem.Time;

namespace Freem.Api.Public.Contracts.V1.Records;

/// <summary>A period of a record: a time interval with an optional description.</summary>
public sealed class RecordPeriodItem
{
    /// <summary>Start and end of the period. The duration must be positive.</summary>
    public DateTimePeriod Period { get; }

    /// <summary>Optional period description.</summary>
    public RecordPeriodDescription? Description { get; init; }

    public RecordPeriodItem(DateTimePeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);

        Period = period;
    }
}
