using Freem.Api.Public.Contracts.V1.Imports.Dtos;

namespace Freem.Api.Public.Contracts.V1.Imports;

/// <summary>Response containing a page of import activities.</summary>
public sealed class ImportActivityListResponse
{
    /// <summary>Total number of import activities matching the query, regardless of paging.</summary>
    public int TotalCount { get; }

    /// <summary>The requested page of import activities.</summary>
    public IReadOnlyList<ImportActivityDto> Activities { get; }

    public ImportActivityListResponse(int totalCount, IReadOnlyList<ImportActivityDto> activities)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);
        ArgumentNullException.ThrowIfNull(activities);

        TotalCount = totalCount;
        Activities = activities;
    }
}
