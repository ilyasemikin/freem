using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions;

public interface IEntity<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    TEntityIdentifier Id { get; }
}
