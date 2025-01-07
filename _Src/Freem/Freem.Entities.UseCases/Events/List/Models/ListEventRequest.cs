using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Events.List.Models;

public sealed class ListEventRequest
{
    public Limit Limit { get; }
    
    public DateTimeOffset? After { get; init; }

    public ListEventRequest()
        : this(Limit.DefaultValue)
    {
    }
    
    public ListEventRequest(Limit limit)
    {
        ArgumentNullException.ThrowIfNull(limit);
        
        Limit = limit;
    }
}