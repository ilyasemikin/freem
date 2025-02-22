using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Users.Events.TelegramIntegrationChanged;

public sealed class UserTelegramIntegrationChangedEvent : EntityEvent<UserIdentifier, UserIdentifier>
{
    public UserTelegramIntegrationChangedEvent(EventIdentifier id, UserIdentifier entityId) 
        : base(id, entityId, entityId, UserEventActions.TelegramIntegrationChanged)
    {
    }
}