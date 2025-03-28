﻿using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Tags.Events.Updated;

public sealed class TagUpdatedEvent : EntityEvent<TagIdentifier, UserIdentifier>
{
    public TagUpdatedEvent(EventIdentifier id, TagIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, TagEventActions.Updated)
    {
    }
}