using Freem.Entities.Identifiers;
using Freem.Entities.Tags.Identifiers;

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