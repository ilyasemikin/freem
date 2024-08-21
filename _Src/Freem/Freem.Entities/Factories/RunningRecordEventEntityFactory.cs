using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers.Factories;
using Freem.Entities.Events;
using Freem.Entities.Factories.Base;
using Freem.Entities.Identifiers;
using Freem.Time.Abstractions;

namespace Freem.Entities.Factories;

public sealed class RunningRecordEventEntityFactory : BaseEventEntityFactory<RunningRecordEvent, RunningRecord, UserIdentifier>
{
    public RunningRecordEventEntityFactory(
        IEntityIdentifierFactory<EventIdentifier> identifierFactory, 
        ICurrentTimeGetter currentTimeGetter) 
        : base(identifierFactory, currentTimeGetter, Create)
    {
    }

    private static RunningRecordEvent Create(
        RunningRecord entity, 
        EventAction action, 
        EventIdentifier id, 
        DateTimeOffset now)
    {
        return new RunningRecordEvent(id, entity.UserId, action, now);
    }
}