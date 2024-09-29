using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Events.Models;
using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions.Events;

public interface IEntityEvent<out TEntityIdentifier, out TUserEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
    where TUserEntityIdentifier : IEntityIdentifier
{
    EventIdentifier Id { get; }
    TEntityIdentifier EntityId { get; }
    TUserEntityIdentifier UserEntityId { get; }

    EventAction Action { get; }
}

public interface IEntityEvent<out TEntityIdentifier, out TUserEntityIdentifier, out TAdditionalData> 
    : IEntityEvent<TEntityIdentifier, TUserEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
    where TUserEntityIdentifier : IEntityIdentifier
    where TAdditionalData : class
{
    TAdditionalData Data { get; }
}