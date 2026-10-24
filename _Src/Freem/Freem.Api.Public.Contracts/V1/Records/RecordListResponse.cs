using Freem.Api.Public.Contracts.V1.Records.Dtos;

namespace Freem.Api.Public.Contracts.V1.Records;

/// <summary>Response containing a page of records.</summary>
public sealed class RecordListResponse
{
    /// <summary>Total number of records matching the query, regardless of paging.</summary>
    public int TotalCount { get; }

    /// <summary>The requested page of records.</summary>
    public IReadOnlyList<RecordDto> Records { get; }

    public RecordListResponse(int totalCount, IReadOnlyList<RecordDto> records)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);
        ArgumentNullException.ThrowIfNull(records);

        TotalCount = totalCount;
        Records = records;
    }
}
