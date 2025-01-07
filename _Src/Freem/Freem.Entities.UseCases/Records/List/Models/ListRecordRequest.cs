using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Records.List.Models;

public sealed class ListRecordRequest
{
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