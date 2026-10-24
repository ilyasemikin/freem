using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Tags.Update;

public sealed class UpdateTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.Update;

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