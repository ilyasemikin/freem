using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers.Factories;
using Freem.Entities.Events;
using Freem.Entities.Factories.Base;
using Freem.Entities.Identifiers;
using Freem.Time.Abstractions;

namespace Freem.Entities.Factories;

public sealed class CategoryEventEntityFactory : BaseEventEntityFactory<CategoryEvent, Category>
{
    public CategoryEventEntityFactory(
        IEntityIdentifierFactory<EventIdentifier> identifierFactory, 
        ICurrentTimeGetter currentTimeGetter) 
        : base(identifierFactory, currentTimeGetter, Create)
    {
    }

    private static CategoryEvent Create(Category entity, EventAction action, EventIdentifier id, DateTimeOffset now)
    {
        return new CategoryEvent(id, entity.UserId, entity.Id, action, now);
    }
}