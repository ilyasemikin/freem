using Freem.Api.Public.Contracts.V1.DataTransfer.Import.Dtos;

namespace Freem.Api.Public.Contracts.V1.DataTransfer.Import;

/// <summary>Response containing a page of import rows.</summary>
public sealed class ImportRowListResponse
{
    /// <summary>Total number of import rows matching the query, regardless of paging.</summary>
    public int TotalCount { get; }

    /// <summary>The requested page of import rows.</summary>
    public IReadOnlyList<ImportRowDto> Rows { get; }

    public ImportRowListResponse(int totalCount, IReadOnlyList<ImportRowDto> rows)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);
        ArgumentNullException.ThrowIfNull(rows);

        TotalCount = totalCount;
        Rows = rows;
    }
}
