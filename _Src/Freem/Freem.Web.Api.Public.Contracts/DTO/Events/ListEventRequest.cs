namespace Freem.Web.Api.Public.Contracts.DTO.Events;

public sealed class ListEventRequest
{
    public const int DefaultLimit = 100;
    
    public int Limit { get; }
    
    public DateTimeOffset? After { get; }

    public ListEventRequest(int limit, DateTimeOffset? after = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);
        
        Limit = limit;
        After = after;
    }
}