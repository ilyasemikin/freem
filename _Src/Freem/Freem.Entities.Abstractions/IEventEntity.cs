using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions;

public interface IEventEntity<out TIdentifier, out TUserIdentifier> : IEntity<TIdentifier>
    where TIdentifier : notnull, IEntityIdentifier
    where TUserIdentifier : notnull, IEntityIdentifier
{
    TIdentifier EventId { get; }
    TUserIdentifier UserId { get; }
    IEntityIdentifier ChangedEntityId { get; }
    EventAction Action { get; }
    DateTimeOffset CreatedAt { get; }

    TIdentifier IEntity<TIdentifier>.Id => EventId;
}