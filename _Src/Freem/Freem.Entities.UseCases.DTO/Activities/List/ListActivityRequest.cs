using Freem.Entities.UseCases.DTO.Abstractions.Models.Filter;

namespace Freem.Entities.UseCases.DTO.Activities.List;

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