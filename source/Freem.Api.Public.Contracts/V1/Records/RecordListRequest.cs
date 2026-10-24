namespace Freem.Api.Public.Contracts.V1.Records;

/// <summary>Paging query for listing records.</summary>
public sealed class RecordListRequest
{
    /// <summary>Offset used when the query omits it.</summary>
    public const int DefaultOffset = 0;
    /// <summary>Limit used when the query omits it.</summary>
    public const int DefaultLimit = 100;
    
    /// <summary>Zero-based number of items to skip.</summary>
    public int Offset { get; }
    /// <summary>Maximum number of items to return. Must be positive.</summary>
    public int Limit { get; }

    public RecordListRequest(int offset, int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);
        
        Offset = offset;
        Limit = limit;
    }
}