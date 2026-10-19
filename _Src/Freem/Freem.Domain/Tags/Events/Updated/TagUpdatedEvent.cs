using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Tags.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Tags.Events.Updated;

public sealed class TagUpdatedEvent : EntityEvent<TagIdentifier, UserIdentifier>
{
    public TagUpdatedEvent(EventIdentifier id, TagIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, TagEventActions.Updated)
    {
    }
}