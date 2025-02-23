namespace Freem.Web.Api.Public.Contracts.Events;

public sealed class ListEventRequest
{
    public int Limit { get; }

    public ListEventRequest(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);
        
        Limit = limit;
    }
}