namespace Freem.Web.Api.Public.Contracts.Activities;

public sealed class ListActivityRequest
{
    public int Limit { get; }
    public int Offset { get; }
    
    public ListActivityRequest(int limit, int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Limit);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        
        Limit = limit;
        Offset = offset;
    }
}