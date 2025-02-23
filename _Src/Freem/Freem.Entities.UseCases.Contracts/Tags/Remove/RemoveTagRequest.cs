using Freem.Entities.Identifiers;

namespace Freem.Entities.UseCases.Contracts.Tags.Remove;

public sealed class RemoveTagRequest
{
    public TagIdentifier Id { get; }

    public RemoveTagRequest(TagIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}