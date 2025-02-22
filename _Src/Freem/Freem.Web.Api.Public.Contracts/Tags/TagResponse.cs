using Freem.Entities.Identifiers;
using Freem.Entities.Models.Tags;

namespace Freem.Web.Api.Public.Contracts.Tags;

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