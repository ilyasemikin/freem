using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Tags.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Tags.Events.Removed;

public sealed class TagRemovedEvent : EntityEvent<TagIdentifier, UserIdentifier>
{
    public TagRemovedEvent(EventIdentifier id, TagIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, TagEventActions.Removed)
    {
    }
}