using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;

internal sealed class RunningRecordEventEntity : BaseEventEntity
{
    public override string EventType { get; protected init; } = EntitiesNames.Events.RunningRecords.EventType;
}