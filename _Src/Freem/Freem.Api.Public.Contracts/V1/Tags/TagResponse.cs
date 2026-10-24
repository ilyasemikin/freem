using Freem.Api.Public.Contracts.V1.Tags.Dtos;

namespace Freem.Api.Public.Contracts.V1.Tags;

/// <summary>Response containing a tag.</summary>
public sealed class TagResponse
{
    /// <summary>The tag.</summary>
    public TagDto Tag { get; }

    public TagResponse(TagDto tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        Tag = tag;
    }
}
