using Freem.Domain.UseCases.Contracts.Filter;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Activities.List;

public sealed class ListActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.List;

    public Limit Limit { get; }
    public Offset Offset { get; }

    public ListActivityRequest()
        : this(Limit.DefaultValue, Offset.DefaultValue)
    {
    }
    
    public ListActivityRequest(Limit limit, Offset offset)
    {
        Limit = limit;
        Offset = offset;
    }
}