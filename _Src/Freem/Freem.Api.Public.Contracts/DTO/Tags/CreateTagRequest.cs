using Freem.Domain.Tags.Models;

namespace Freem.Api.Public.Contracts.DTO.Tags;

/// <summary>Request body for creating a tag.</summary>
public sealed class CreateTagRequest
{
    /// <summary>Tag name.</summary>
    public TagName Name { get; }

    public CreateTagRequest(TagName name)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        Name = name;
    }
}