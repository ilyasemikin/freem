using Freem.Entities.Identifiers;
using Freem.Entities.Models.Tags;

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