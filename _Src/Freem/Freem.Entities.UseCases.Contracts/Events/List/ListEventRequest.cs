using Freem.Entities.UseCases.Contracts.Filter;

namespace Freem.Entities.UseCases.Contracts.Events.List;

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