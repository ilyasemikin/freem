﻿using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Abstractions.Relations.Collection.Exceptions;

namespace Freem.Entities.Relations.Collections.Base;

public class RelatedEntitiesCollection<TEntity, TEntityIdentifier> 
    : IRelatedEntitiesCollection<TEntity, TEntityIdentifier>
    where TEntity : IEntity<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    private readonly Dictionary<TEntityIdentifier, TEntity?> _entities;

    public const int DefaultMinCount = 0;
    public const int DefaultMaxCount = int.MaxValue;
    
    public int Count => _entities.Count;

    public IEnumerable<TEntityIdentifier> Identifiers => _entities.Keys;
    public IEnumerable<TEntity> Entities => _entities.Values.Where(e => e is not null)!;

    public int MinCount { get; }
    public int MaxCount { get; }

    public RelatedEntitiesCollection(
        IEnumerable<TEntityIdentifier>? identifiers = null,
        IEnumerable<TEntity>? entities = null,
        int minCount = DefaultMinCount, 
        int maxCount = DefaultMaxCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(minCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxCount);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(minCount, maxCount);

        identifiers ??= [];
        entities ??= [];

        MinCount = minCount;
        MaxCount = maxCount;

        _entities = new Dictionary<TEntityIdentifier, TEntity?>();
        FillEntities(_entities, identifiers, entities);

        if (!IsCountAllowed(_entities.Count))
            throw new InvalidRelatedCountException(MinCount, MaxCount, _entities.Count);
    }

    public bool TryAdd(TEntityIdentifier identifier)
    {
        return TryAdd(identifier, default);
    }

    public bool TryAdd(TEntity entity)
    {
        return TryAdd(entity.Id, entity);
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
        return IsCountAllowed(Count - 1) && _entities.Remove(identifier);
    }

    public bool TryGet(TEntityIdentifier identifier, [NotNullWhen(true)] out TEntity? entity)
    {
        return _entities.TryGetValue(identifier, out entity) && entity is not null;
    }

    public bool Contains(TEntityIdentifier identifier)
    {
        return _entities.ContainsKey(identifier);
    }

    protected void Update(IReadOnlyRelatedEntitiesCollection<TEntity, TEntityIdentifier> other)
    {
        _entities.Clear();

        FillEntities(_entities, other.Identifiers, other.Entities);
    }

    private bool TryAdd(TEntityIdentifier identifier, TEntity? entity)
    {
        if (_entities.ContainsKey(identifier) || !IsCountAllowed(Count + 1))
            return false;

        _entities.Add(identifier, entity);
        return true;
    }
    
    private bool IsCountAllowed(int newCount)
    {
        return newCount >= MinCount && newCount <= MaxCount;
    }

    private static void FillEntities(
        IDictionary<TEntityIdentifier, TEntity?> dictionary,
        IEnumerable<TEntityIdentifier> identifiers, 
        IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (!dictionary.TryAdd(entity.Id, entity))
                throw new InvalidOperationException($"Entity with id \"{entity.Id}\" already added");
        }

        foreach (var identifier in identifiers)
            dictionary.TryAdd(identifier, default);
    }
}
