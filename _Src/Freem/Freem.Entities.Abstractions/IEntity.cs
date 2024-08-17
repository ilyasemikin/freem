using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions;

public interface IEntity<out TIdentifier>
    where TIdentifier : notnull, IEntityIdentifier
{
    TIdentifier Id { get; }
}
