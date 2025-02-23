using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Events.Models;
using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Web.Api.Public.Contracts.Events;

public sealed class EventResponse
{
    public EventIdentifier Id { get; }
    public IEntityIdentifier EntityId { get; }
    public EventAction Action { get; }

    public EventResponse(EventIdentifier id, IEntityIdentifier entityId, EventAction action)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(entityId);
        ArgumentNullException.ThrowIfNull(action);
        
        Id = id;
        EntityId = entityId;
        Action = action;
    }
}