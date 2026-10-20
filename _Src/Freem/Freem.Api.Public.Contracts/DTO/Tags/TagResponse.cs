using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;

namespace Freem.Api.Public.Contracts.DTO.Tags;

/// <summary>A tag.</summary>
public sealed class TagResponse
{
    /// <summary>Tag id.</summary>
    public TagId Id { get; }
    
    /// <summary>Tag name.</summary>
    public TagName Name { get; }

    public TagResponse(TagId id, TagName name)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);
        
        Id = id;
        Name = name;
    }
}