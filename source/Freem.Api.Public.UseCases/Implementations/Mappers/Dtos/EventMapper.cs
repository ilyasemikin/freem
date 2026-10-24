using Freem.Api.Public.Contracts.V1.Events.Dtos;
using Freem.Api.Public.Contracts.V1.Events;
using Freem.Domain.Entities.Events;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class EventMapper
{
    public static EventDto Map(this Event @event)
    {
        return new EventDto(@event.Id, @event.Type, @event.EntityRef, @event.OccurredAt, @event.Payload);
    }
}
