using Freem.Time.Models;

namespace Freem.Web.Api.Public.Contracts.Records;

public sealed class ListByPeriodRequest
{
    public int Limit { get; }
    public DateTimePeriod Period { get; }

    public ListByPeriodRequest(int limit, DateTimePeriod period)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);
        ArgumentNullException.ThrowIfNull(period);
        
        Limit = limit;
        Period = period;
    }
}