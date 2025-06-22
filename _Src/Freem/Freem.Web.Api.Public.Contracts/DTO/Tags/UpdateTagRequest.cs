using Freem.Entities.Tags.Models;
using Freem.Web.Api.Public.Contracts.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Tags;

public sealed class UpdateTagRequest
{
    public UpdateField<TagName>? Name { get; init; }

    public bool HasChanges()
    {
        return Name is not null;
    }
}