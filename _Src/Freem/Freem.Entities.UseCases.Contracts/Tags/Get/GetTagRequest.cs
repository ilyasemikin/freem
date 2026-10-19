using Freem.Domain.Tags;

namespace Freem.Entities.UseCases.Contracts.Tags.Get;

public sealed class GetTagRequest
{
    public TagId Id { get; }

    public GetTagRequest(TagId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}