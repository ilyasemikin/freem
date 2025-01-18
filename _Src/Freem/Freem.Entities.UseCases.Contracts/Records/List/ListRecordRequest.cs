using Freem.Entities.UseCases.Contracts.Filter;

namespace Freem.Entities.UseCases.Contracts.Records.List;

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