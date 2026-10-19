using Freem.Domain.Identifiers;

namespace Freem.Domain;

public interface IEntity<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    TEntityIdentifier Id { get; }
}
