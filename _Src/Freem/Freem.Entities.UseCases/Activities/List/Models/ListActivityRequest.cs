using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Activities.List.Models;

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