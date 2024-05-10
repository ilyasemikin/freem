using Freem.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Collections.Extensions;

public static class RelatedEntitiesCollectionExtensions
{
    public static bool Contains<TEntity>(this RelatedEntitiesCollection<TEntity> collection, TEntity entity)
        where TEntity : class, IEntity
    {
        ArgumentNullException.ThrowIfNull(collection);

        return collection.Contains(entity.Id);
    }

    public static bool ContainsEntities<TEntity>(this RelatedEntitiesCollection<TEntity> collection, string identifier)
        where TEntity : class, IEntity
    {
        ArgumentNullException.ThrowIfNull(collection);

        return collection.TryGet(identifier, out var _);
    }
}
