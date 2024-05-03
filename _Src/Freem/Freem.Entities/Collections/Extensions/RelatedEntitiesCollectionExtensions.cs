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
        return collection.Contains(entity.Id);
    }

    public static bool ContainsEntitys<TEntity>(this RelatedEntitiesCollection<TEntity> collection, string id)
        where TEntity : class, IEntity
    {
        return collection.TryGet(id, out var _);
    }
}
