using Freem.Time.Models;

namespace Freem.Web.Api.Public.Contracts.Records;

public sealed class ListRecordByPeriodRequest
{
    public const int DefaultLimit = 100;
    
    public DateTimePeriod Period { get; }
    public int Limit { get; }

    public ListRecordByPeriodRequest(DateTimePeriod period, int limit)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);
        
        Period = period;
        Limit = limit;
    }
}