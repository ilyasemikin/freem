using Freem.Api.Public.Contracts.V1.Events.Dtos;

namespace Freem.Api.Public.Contracts.V1.Events;

/// <summary>Response containing an event.</summary>
public sealed class EventResponse
{
    /// <summary>The event.</summary>
    public EventDto Event { get; }

    public EventResponse(EventDto @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        Event = @event;
    }
}
