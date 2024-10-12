using Freem.Entities.Tags.Identifiers;

namespace Freem.Entities.UseCases.Tags.Remove.Models;

public sealed class RemoveTagRequest
{
    public TagIdentifier Id { get; }

    public RemoveTagRequest(TagIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}