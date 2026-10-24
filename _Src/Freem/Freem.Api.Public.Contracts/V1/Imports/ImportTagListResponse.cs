using Freem.Api.Public.Contracts.V1.Imports.Dtos;

namespace Freem.Api.Public.Contracts.V1.Imports;

/// <summary>Response containing a page of import tags.</summary>
public sealed class ImportTagListResponse
{
    /// <summary>Total number of import tags matching the query, regardless of paging.</summary>
    public int TotalCount { get; }

    /// <summary>The requested page of import tags.</summary>
    public IReadOnlyList<ImportTagDto> Tags { get; }

    public ImportTagListResponse(int totalCount, IReadOnlyList<ImportTagDto> tags)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);
        ArgumentNullException.ThrowIfNull(tags);

        TotalCount = totalCount;
        Tags = tags;
    }
}
