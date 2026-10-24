using Freem.Api.Public.Contracts.V1.DataTransfer.Import.Dtos;

namespace Freem.Api.Public.Contracts.V1.DataTransfer.Import;

/// <summary>Response containing a page of imports.</summary>
public sealed class ImportListResponse
{
    /// <summary>Total number of imports matching the query, regardless of paging.</summary>
    public int TotalCount { get; }

    /// <summary>The requested page of imports.</summary>
    public IReadOnlyList<ImportDto> Imports { get; }

    public ImportListResponse(int totalCount, IReadOnlyList<ImportDto> imports)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);
        ArgumentNullException.ThrowIfNull(imports);

        TotalCount = totalCount;
        Imports = imports;
    }
}
