using Freem.Entities.UseCases.Contracts.Filter;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Contracts.Records.List;

public sealed class ListRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.List;

    public Limit Limit { get; }
    public Offset Offset { get; }

    public ListRecordRequest()
        : this(Limit.DefaultValue, Offset.DefaultValue)
    {
    }
    
    public ListRecordRequest(Limit limit, Offset offset)
    {
        Limit = limit;
        Offset = offset;
    }
}