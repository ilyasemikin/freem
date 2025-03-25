using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.Contracts.Records.PeriodList;

public sealed class PeriodListRequest
{
    public Limit Limit { get; }
    public DateTimePeriod Period { get; }

    public PeriodListRequest(DateTimePeriod period)
        : this(period, Limit.DefaultValue)
    {
        
    }
    
    public PeriodListRequest(DateTimePeriod period, Limit limit)
    {
        Period = period;
        Limit = limit;
    }
}