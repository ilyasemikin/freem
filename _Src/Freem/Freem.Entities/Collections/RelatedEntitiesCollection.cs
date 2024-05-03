using Freem.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Collections;

public sealed class RelatedEntitiesCollection<TEntity>
    where TEntity : class, IEntity
{
    private readonly IDictionary<string, TEntity?> _entities;

    public int Count => _entities.Count;

    public int MinCount { get; }

    public RelatedEntitiesCollection(IEnumerable<TEntity>? entities = null, int minSize = 0)
    {
        _entities = entities?.

        MinCount = minSize;
    }

    public bool TryAdd(string id)
    {
        return _entities.TryAdd(id, default);
    }

    public bool TryAdd(TEntity entity)
    {
        return _entities.TryAdd(entity.Id, entity);
    }

    public bool TryRemove(string id)
    {
        if (Count == MinCount && Contains(id))
            return false;

        return _entities.Remove(id);
    }

    public bool Contains(string id)
    {
        return _entities.ContainsKey(id);
    }

    public bool TryGet(string id, [NotNullWhen(true)] out TEntity? entity)
    {
        return _entities.TryGetValue(id, out entity) && entity is not null;
    }
}
