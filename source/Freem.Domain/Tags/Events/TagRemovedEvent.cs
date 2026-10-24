using Freem.Domain.Events;

namespace Freem.Domain.Tags.Events;

public sealed class TagRemovedEvent : EntityEvent
{
    public TagRemovedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.Tags.Removed;

    public override EntityRef EntityRef => new(EntityType.Tag, TagId.ToString());

    public required TagId TagId { get; init; }
}
