using Freem.Entities.Identifiers;

namespace Freem.Entities.UseCases.Contracts.Tags.Get;

public sealed class GetTagRequest
{
    public TagIdentifier Id { get; }

    public GetTagRequest(TagIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}