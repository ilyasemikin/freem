using Freem.Domain.Events;
using Freem.Domain.Tags.Models;

namespace Freem.Domain.Tags.Events;

public sealed class TagUpdatedEvent : EntityEvent
{
    public TagUpdatedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.Tags.Updated;

    public override EntityRef EntityRef => new(EntityType.Tag, TagId.ToString());

    public required TagId TagId { get; init; }
    public required TagName Name { get; init; }
}
