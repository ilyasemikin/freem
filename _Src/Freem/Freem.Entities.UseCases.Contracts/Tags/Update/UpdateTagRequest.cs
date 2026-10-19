using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;

namespace Freem.Entities.UseCases.Contracts.Tags.Update;

public sealed class UpdateTagRequest
{
    public TagId Id { get; }
    
    public UpdateField<TagName>? Name { get; init; }

    public UpdateTagRequest(TagId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }

    public bool HasChanges()
    {
        return Name is not null;
    }
}