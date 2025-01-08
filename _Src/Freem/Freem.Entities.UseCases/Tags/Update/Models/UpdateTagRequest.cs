using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Tags.Models;
using Freem.Entities.UseCases.Models.Fields;

namespace Freem.Entities.UseCases.Tags.Update.Models;

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