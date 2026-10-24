using Freem.Domain.Tags.Models;

namespace Freem.Api.Public.Contracts.V1.Tags;

/// <summary>Request body for creating a tag.</summary>
public sealed class TagCreateRequest
{
    /// <summary>Tag name.</summary>
    public TagName Name { get; }

    public TagCreateRequest(TagName name)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        Name = name;
    }
}