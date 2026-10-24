using Freem.Domain.Events;
using Freem.Domain.Users.Models;

namespace Freem.Domain.Users.Events;

public sealed class UserSettingsChangedEvent : EntityEvent
{
    public UserSettingsChangedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.Users.SettingsChanged;

    public override EntityRef EntityRef => new(EntityType.User, UserId.ToString());

    public required UserId UserId { get; init; }
    public required TimeZoneOffset TimeZoneOffset { get; init; }
}
