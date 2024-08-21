using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers.Factories;
using Freem.Entities.Events;
using Freem.Entities.Factories.Base;
using Freem.Entities.Identifiers;
using Freem.Time.Abstractions;

namespace Freem.Entities.Factories;

public sealed class RecordEventEntityFactory : BaseEventEntityFactory<RecordEvent, Record, RecordIdentifier>
{
    public RecordEventEntityFactory(
        IEntityIdentifierFactory<EventIdentifier> identifierFactory, 
        ICurrentTimeGetter currentTimeGetter) 
        : base(identifierFactory, currentTimeGetter, Create)
    {
    }

    private static RecordEvent Create(Record entity, EventAction action, EventIdentifier id, DateTimeOffset now)
    {
        return new RecordEvent(id, entity.UserId, entity.Id, action, now);
    }
}