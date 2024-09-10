using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;

internal sealed class ActivityEventEntity : BaseEventEntity
{
    public required string ActivityId { get; init; }
    
    public override string EventType { get; protected init; } = EntitiesNames.Events.Activities.EventType;
}