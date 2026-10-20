using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;

namespace Freem.Api.Public.Contracts.DTO.Tags;

public sealed class TagResponse
{
    public TagId Id { get; }
    
    public TagName Name { get; }

    public TagResponse(TagId id, TagName name)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);
        
        Id = id;
        Name = name;
    }
}