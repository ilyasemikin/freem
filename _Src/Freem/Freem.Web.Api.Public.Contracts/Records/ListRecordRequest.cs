namespace Freem.Web.Api.Public.Contracts.Records;

public sealed class ListRecordRequest
{
    public const int DefaultOffset = 0;
    public const int DefaultLimit = 100;
    
    public int Offset { get; }
    public int Limit { get; }

    public ListRecordRequest(int offset, int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);
        
        Offset = offset;
        Limit = limit;
    }
}