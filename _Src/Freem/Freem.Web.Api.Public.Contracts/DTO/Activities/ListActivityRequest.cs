namespace Freem.Web.Api.Public.Contracts.DTO.Activities;

public sealed class ListActivityRequest
{
    public const int DefaultOffset = 0;
    public const int DefaultLimit = 100;
    
    public int Offset { get; }
    public int Limit { get; }
    
    public ListActivityRequest(int offset, int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);
        
        Offset = offset;
        Limit = limit;
    }
}