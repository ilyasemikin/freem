using Freem.Time;

namespace Freem.Api.Public.Contracts.V1.Records;

/// <summary>Query for listing records within a period.</summary>
public sealed class RecordListByPeriodRequest
{
    /// <summary>Limit used when the query omits it.</summary>
    public const int DefaultLimit = 100;
    
    /// <summary>Period to take records from.</summary>
    public DateTimePeriod Period { get; }
    /// <summary>Maximum number of items to return. Must be positive.</summary>
    public int Limit { get; }

    public RecordListByPeriodRequest(DateTimePeriod period, int limit)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);
        
        Period = period;
        Limit = limit;
    }
}