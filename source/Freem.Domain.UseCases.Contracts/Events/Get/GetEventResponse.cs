using Freem.Domain.Entities.Events;

namespace Freem.Domain.UseCases.Contracts.Events.Get;

public sealed class GetEventResponse
{
    public Event Event { get; }

    public GetEventResponse(Event @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        Event = @event;
    }
}
