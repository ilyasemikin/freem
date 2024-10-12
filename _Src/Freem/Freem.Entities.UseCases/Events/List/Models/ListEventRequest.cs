using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Events.List.Models;

public sealed class ListEventRequest
{
    public Limit Limit { get; init; }
    
    public DateTimeOffset? After { get; init; }
}