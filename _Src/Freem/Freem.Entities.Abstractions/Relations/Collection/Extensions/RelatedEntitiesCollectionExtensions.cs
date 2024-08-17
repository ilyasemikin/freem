using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions.Relations.Collection.Extensions;

public static class RelatedEntitiesCollectionExtensions
{
    public static bool Contains<TEntity, TEntityIdentifier>(
        this IReadOnlyRelatedEntitiesCollection<TEntity, TEntityIdentifier> collection, 
        TEntityIdentifier identifier)
            where TEntity : notnull, IEntity<TEntityIdentifier>
            where TEntityIdentifier : notnull, IEntityIdentifier
    {
        return collection.TryGet(identifier, out _);
    }
}
