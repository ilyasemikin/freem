using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;

namespace Freem.Api.Public.Contracts.V1.Tags.Dtos;

/// <summary>A tag.</summary>
public sealed class TagDto
{
    /// <summary>Tag id.</summary>
    public TagId Id { get; }
    
    /// <summary>Tag name.</summary>
    public TagName Name { get; }

    public TagDto(TagId id, TagName name)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);
        
        Id = id;
        Name = name;
    }
}