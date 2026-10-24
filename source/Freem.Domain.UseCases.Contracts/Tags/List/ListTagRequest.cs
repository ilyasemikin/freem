using Freem.Domain.UseCases.Contracts.Filter;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Tags.List;

public sealed class ListTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.List;

    public Limit Limit { get; }
    public Offset Offset { get; }

    public ListTagRequest()
        : this(Limit.DefaultValue, Offset.DefaultValue)
    {
    }
    
    public ListTagRequest(Limit limit, Offset offset)
    {
        Limit = limit;
        Offset = offset;
    }
}