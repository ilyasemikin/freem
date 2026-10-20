using Freem.Domain.UseCases.Contracts.Filter;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Events.List;

public sealed class ListEventRequest : IRequest
{
    public RequestType Type => RequestTypes.Events.List;

    public Limit Limit { get; }
    
    public DateTimeOffset? After { get; init; }

    public ListEventRequest()
        : this(Limit.DefaultValue)
    {
    }
    
    public ListEventRequest(Limit limit)
    {
        Limit = limit;
    }
}