using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Tags.Models;

namespace Freem.Entities.UseCases.Contracts.Tags.Update;

public sealed class UpdateTagRequest
{
    public TagIdentifier Id { get; }
    
    public UpdateField<TagName>? Name { get; init; }

    public UpdateTagRequest(TagIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }

    public bool HasChanges()
    {
        return Name is not null;
    }
}