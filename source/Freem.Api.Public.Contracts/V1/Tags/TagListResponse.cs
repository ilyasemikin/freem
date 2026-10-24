using Freem.Api.Public.Contracts.V1.Tags.Dtos;

namespace Freem.Api.Public.Contracts.V1.Tags;

/// <summary>Response containing a page of tags.</summary>
public sealed class TagListResponse
{
    /// <summary>Total number of tags matching the query, regardless of paging.</summary>
    public int TotalCount { get; }

    /// <summary>The requested page of tags.</summary>
    public IReadOnlyList<TagDto> Tags { get; }

    public TagListResponse(int totalCount, IReadOnlyList<TagDto> tags)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);
        ArgumentNullException.ThrowIfNull(tags);

        TotalCount = totalCount;
        Tags = tags;
    }
}
