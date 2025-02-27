using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Tags.Events.Created;

public sealed class TagCreatedEvent : EntityEvent<TagIdentifier, UserIdentifier>
{
    public TagCreatedEvent(EventIdentifier id, TagIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, TagEventActions.Created)
    {
    }
}