using Freem.Entities.UseCases.DTO.Abstractions.Models.Filter;

namespace Freem.Entities.UseCases.DTO.Tags.List;

public sealed class ListTagRequest
{
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