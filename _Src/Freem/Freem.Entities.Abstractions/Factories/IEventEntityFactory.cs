using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions.Factories;

public interface IEventEntityFactory<TEventEntity, TEventIdentifier, TUserIdentifier, TEntity, TEntityIdentifier>
    where TEventEntity : IEventEntity<TEventIdentifier, TUserIdentifier>
    where TEventIdentifier : IEntityIdentifier
    where TUserIdentifier : IEntityIdentifier
    where TEntity : IEntity<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    TEventEntity Create(TEntity entity);
}