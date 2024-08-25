using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers.Factories;
using Freem.Entities.Events;
using Freem.Entities.Factories.Base;
using Freem.Entities.Identifiers;
using Freem.Time.Abstractions;

namespace Freem.Entities.Factories;

public class TagEventEntityFactory : BaseEventEntityFactory<TagEvent, Tag>
{
    public TagEventEntityFactory(
        IEntityIdentifierFactory<EventIdentifier> identifierFactory, 
        ICurrentTimeGetter currentTimeGetter) 
        : base(identifierFactory, currentTimeGetter, Create)
    {
    }

    private static TagEvent Create(Tag entity, EventAction action, EventIdentifier id, DateTimeOffset now)
    {
        return new TagEvent(id, entity.UserId, entity.Id, action, now);
    }
}