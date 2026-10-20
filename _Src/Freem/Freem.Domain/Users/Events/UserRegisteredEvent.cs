using Freem.Domain.Events;
using Freem.Domain.Users.Models;

namespace Freem.Domain.Users.Events;

public sealed class UserRegisteredEvent : EntityEvent
{
    public UserRegisteredEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.Users.Registered;

    public override EntityRef EntityRef => new(EntityType.User, UserId.ToString());

    public required UserId UserId { get; init; }
    public required Nickname Nickname { get; init; }
}
