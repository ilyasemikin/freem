using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions.Relations.Collection;

public interface IReadOnlyRelatedEntitiesCollection<TEntity, TEntityIdentifier>
    where TEntity : IEntity<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    int Count { get; }

    IEnumerable<TEntityIdentifier> Identifiers { get; }
    IEnumerable<TEntity> Entities { get; }

    bool TryGet(TEntityIdentifier identifier, [NotNullWhen(true)] out TEntity? entity);
}
