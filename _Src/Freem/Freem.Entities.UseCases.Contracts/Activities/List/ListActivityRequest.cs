using Freem.Entities.UseCases.Contracts.Filter;

namespace Freem.Entities.UseCases.Contracts.Activities.List;

public sealed class ListActivityRequest
{
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