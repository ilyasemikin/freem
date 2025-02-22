using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Entities.Tags.Events.Removed;

public sealed class TagRemovedEvent : EntityEvent<TagIdentifier, UserIdentifier>
{
    public TagRemovedEvent(EventIdentifier id, TagIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, TagEventActions.Removed)
    {
    }
}