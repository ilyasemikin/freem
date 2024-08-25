using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions.Factories;

public interface IEventEntityFactory<TEventEntity, TEntity>
    where TEventEntity : IEventEntity<IEntityIdentifier, IEntityIdentifier>
    where TEntity : IEntity<IEntityIdentifier>
{
    TEventEntity Create(TEntity entity, EventAction action);
}