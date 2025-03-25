using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions.Relations.Collection;

public interface IReadOnlyRelatedEntitiesCollection<TEntity, TEntityIdentifier>
    : IReadOnlyRelatedEntitiesIdentifiersCollection<TEntityIdentifier>
    where TEntity : IEntity<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    IEnumerable<TEntity> Entities { get; }

    bool TryGet(TEntityIdentifier identifier, [NotNullWhen(true)] out TEntity? entity);

    static bool Equals(
        IReadOnlyRelatedEntitiesCollection<TEntity, TEntityIdentifier> x, 
        IReadOnlyRelatedEntitiesCollection<TEntity, TEntityIdentifier> y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Count != y.Count)
            return false;

        var pairs = x.Identifiers.Zip(y.Identifiers);
        foreach (var (xItem, yItem) in pairs)
            if (!xItem.Equals(yItem))
                return false;

        return true;
    }
    
    static bool Equals(
        IReadOnlyRelatedEntitiesCollection<TEntity, TEntityIdentifier> x, 
        IReadOnlyRelatedEntitiesCollection<TEntity, TEntityIdentifier> y,
        IEqualityComparer<TEntityIdentifier> comparer)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Count != y.Count)
            return false;

        var pairs = x.Identifiers.Zip(y.Identifiers);
        foreach (var (xItem, yItem) in pairs)
            if (!comparer.Equals(xItem, yItem))
                return false;

        return true;
    }
}
