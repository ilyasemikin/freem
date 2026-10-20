using Freem.Domain.Entities.Events;
using Freem.Api.Public.Contracts.DTO.Events;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class EventMapper
{
    public static EventResponse Map(this Event @event)
    {
        return new EventResponse(@event.Id, @event.Type, @event.EntityRef, @event.OccurredAt, @event.Payload);
    }
}
