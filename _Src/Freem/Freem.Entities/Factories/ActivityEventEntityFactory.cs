using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers.Factories;
using Freem.Entities.Events;
using Freem.Entities.Factories.Base;
using Freem.Entities.Identifiers;
using Freem.Time.Abstractions;

namespace Freem.Entities.Factories;

public sealed class ActivityEventEntityFactory : BaseEventEntityFactory<ActivityEvent, Activity>
{
    public ActivityEventEntityFactory(
        IEntityIdentifierFactory<EventIdentifier> identifierFactory, 
        ICurrentTimeGetter currentTimeGetter) 
        : base(identifierFactory, currentTimeGetter, Create)
    {
    }

    private static ActivityEvent Create(Activity entity, EventAction action, EventIdentifier id, DateTimeOffset now)
    {
        return new ActivityEvent(id, entity.UserId, entity.Id, action, now);
    }
}