using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Time.Models;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Contracts.Records.PeriodList;

public sealed class PeriodListRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.PeriodList;

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