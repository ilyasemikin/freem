using Freem.Domain.UseCases.Contracts.Filter;
using Freem.Time;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Records.ListByPeriod;

public sealed class ListRecordByPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.ListByPeriod;

    public Limit Limit { get; }
    public DateTimePeriod Period { get; }

    public ListRecordByPeriodRequest(DateTimePeriod period)
        : this(period, Limit.DefaultValue)
    {
        
    }
    
    public ListRecordByPeriodRequest(DateTimePeriod period, Limit limit)
    {
        Period = period;
        Limit = limit;
    }
}