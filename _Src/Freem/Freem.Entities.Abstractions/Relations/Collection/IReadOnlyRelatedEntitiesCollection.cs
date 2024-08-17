using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions.Relations.Collection;

public interface IReadOnlyRelatedEntitiesCollection<TEntity, TEntityIdentifier>
    where TEntity : notnull, IEntity<TEntityIdentifier>
    where TEntityIdentifier : notnull, IEntityIdentifier
{
    int Count { get; }

    IEnumerable<TEntityIdentifier> Identifiers { get; }
    IEnumerable<TEntity> Entities { get; }

    bool TryGet(TEntityIdentifier identifier, [NotNullWhen(true)] out TEntity? entity);
}
