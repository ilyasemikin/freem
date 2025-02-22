namespace Freem.Web.Api.Public.Contracts.Records;

public sealed class ListRecordRequest
{
    public int Limit { get; }
    public int Offset { get; }

    public ListRecordRequest(int limit, int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        
        Limit = limit;
        Offset = offset;
    }
}