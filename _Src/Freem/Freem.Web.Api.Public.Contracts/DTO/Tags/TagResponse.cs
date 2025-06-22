using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Tags.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Tags;

public sealed class TagResponse
{
    public TagIdentifier Id { get; }
    
    public TagName Name { get; }

    public TagResponse(TagIdentifier id, TagName name)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);
        
        Id = id;
        Name = name;
    }
}