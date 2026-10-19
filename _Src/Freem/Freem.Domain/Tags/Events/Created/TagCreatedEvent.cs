using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Tags.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Tags.Events.Created;

public sealed class TagCreatedEvent : EntityEvent<TagIdentifier, UserIdentifier>
{
    public TagCreatedEvent(EventIdentifier id, TagIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, TagEventActions.Created)
    {
    }
}