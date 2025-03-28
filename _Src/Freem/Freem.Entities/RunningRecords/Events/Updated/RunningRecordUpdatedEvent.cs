﻿using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.RunningRecords.Events.Updated;

public sealed class RunningRecordUpdatedEvent : EntityEvent<RunningRecordIdentifier, UserIdentifier>
{
    public RunningRecordUpdatedEvent(EventIdentifier id, RunningRecordIdentifier entityId) 
        : base(id, entityId, entityId, RunningRecordEventActions.Updated)
    {
    }
}