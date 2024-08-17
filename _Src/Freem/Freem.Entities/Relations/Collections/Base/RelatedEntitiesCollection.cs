using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Abstractions.Relations.Collection;

namespace Freem.Entities.Relations.Collections.Base;

public abstract class RelatedEntitiesCollection<TEntity, TEntityIdentifier> : IRelatedEntitiesCollection<TEntity, TEntityIdentifier>
    where TEntity : notnull, IEntity<TEntityIdentifier>
    where TEntityIdentifier : notnull, IEntityIdentifier
{
    private readonly Dictionary<TEntityIdentifier, TEntity?> _entities;

    public int Count => _entities.Count;

    public IEnumerable<TEntityIdentifier> Identifiers => _entities.Keys;
    public IEnumerable<TEntity> Entities => _entities.Values.Where(e => e is not null)!;

    public int MinCount { get; }
    public int MaxCount { get; }

    protected internal RelatedEntitiesCollection(
        IEnumerable<TEntityIdentifier> identifiers,
        IEnumerable<TEntity> entities,
        int minCount = 0, 
        int maxCount = int.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(minCount);
        ArgumentOutOfRangeException.ThrowIfNegative(maxCount);

        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minCount, maxCount);

        _entities = GetEntities(identifiers, entities);

        MinCount = minCount;
        MaxCount = maxCount;
    }

    public bool TryAdd(TEntityIdentifier identifier)
    {
        if (_entities.ContainsKey(identifier) && !IsCountAllowed(Count + 1))
            return false;

        _entities.Add(identifier, default);
        return true;
    }

    public bool TryAdd(TEntity entity)
    {
        if (_entities.ContainsKey(entity.Id) && !IsCountAllowed(Count + 1)) 
            return false;

        _entities.Add(entity.Id, entity);
        return true;
    }

    public bool TryUpdate(TEntity entity)
    {
        if (!_entities.ContainsKey(entity.Id))
            return false;

        _entities[entity.Id] = entity;
        return true;
    }

    public bool TryRemove(TEntityIdentifier identifier)
    {
        if (!_entities.ContainsKey(identifier))
            return false;
        if (IsCountAllowed(Count - 1))
            return false;

        _entities.Remove(identifier);
        return true;
    }

    public bool TryGet(TEntityIdentifier identifier, [NotNullWhen(true)] out TEntity? entity)
    {
        return _entities.TryGetValue(identifier, out entity);
    }

    private bool IsCountAllowed(int newCount)
    {
        return newCount >= MinCount && newCount <= MaxCount;
    }

    private static Dictionary<TEntityIdentifier, TEntity?> GetEntities(
        IEnumerable<TEntityIdentifier> identifiers, 
        IEnumerable<TEntity> entities)
    {
        var result = new Dictionary<TEntityIdentifier, TEntity?>();

        foreach (var entity in entities)
        {
            if (!result.TryAdd(entity.Id, entity))
                throw new InvalidOperationException($"Entity with id \"{entity.Id}\" already added");
        }

        foreach (var identifier in identifiers)
            result.TryAdd(identifier, default);

        return result;
    }
}
