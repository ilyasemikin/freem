using Freem.Time.Models;

namespace Freem.Api.Public.Contracts.DTO.Records;

/// <summary>Query for listing records within a period.</summary>
public sealed class ListRecordByPeriodRequest
{
    /// <summary>Limit used when the query omits it.</summary>
    public const int DefaultLimit = 100;
    
    /// <summary>Period to take records from.</summary>
    public DateTimePeriod Period { get; }
    /// <summary>Maximum number of items to return. Must be positive.</summary>
    public int Limit { get; }

    public ListRecordByPeriodRequest(DateTimePeriod period, int limit)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);
        
        Period = period;
        Limit = limit;
    }
}