using Freem.Entities.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace Freem.Entities.Collections.Abstractions;

public abstract class RelatedEntitiesCollection<TEntity>
    where TEntity : class, IEntity
{
    private const string TooFewItemsExceptionMessageTemplate = "To few items. Minimum must be {0}, but get {1}";

    private IDictionary<string, TEntity?> _entities;

    public int Count => _entities.Count;

    public int MinCount { get; }

    public IEnumerable<string> Identifiers => _entities.Keys;
    public IEnumerable<TEntity> Entities => _entities.Values
        .Where(x => x is not null)
        .Select(x => x!);

    protected internal RelatedEntitiesCollection(IEnumerable<TEntity> entities, IEnumerable<string> identifiers, int minCount = 0)
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(identifiers);

        ArgumentOutOfRangeException.ThrowIfNegative(minCount);

        _entities = GroupByIdentifiers(entities, identifiers);

        MinCount = minCount;

        if (_entities.Count < MinCount)
            throw new InvalidOperationException(GetTooFewItemsExceptionMessage(_entities.Count));
    }

    public bool TryAdd(string identifier)
    {
        ArgumentException.ThrowIfNullOrEmpty(identifier);

        return _entities.TryAdd(identifier, default);
    }

    public bool TryAdd(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return _entities.TryAdd(entity.Id, entity);
    }

    public bool TryRemove(string identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        if (Count == MinCount && Contains(identifier))
            return false;

        return _entities.Remove(identifier);
    }

    public void Update(IEnumerable<string> identifiers)
    {
        var entitiesDictionary = GroupByIdentifiers(identifiers);

        if (entitiesDictionary.Count < MinCount)
            throw new InvalidOperationException();

        _entities = entitiesDictionary;
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        var entitiesDictionary = GroupByIdentifiers(entities);

        if (entitiesDictionary.Count < MinCount)
            throw new InvalidOperationException();

        _entities = entitiesDictionary;
    }

    public void Update(IEnumerable<TEntity> entities, IEnumerable<string> identifiers)
    {
        ArgumentNullException.ThrowIfNull(entities);

        var entitiesDictionary = GroupByIdentifiers(entities, identifiers);

        if (entitiesDictionary.Count < MinCount)
            throw new InvalidOperationException();

        _entities = entitiesDictionary;
    }

    public bool Contains(string identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _entities.ContainsKey(identifier);
    }

    public bool TryGet(string identifier, [NotNullWhen(true)] out TEntity? entity)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _entities.TryGetValue(identifier, out entity) && entity is not null;
    }

    private string GetTooFewItemsExceptionMessage(int count)
    {
        return string.Format(TooFewItemsExceptionMessageTemplate, MinCount, count);
    }

    private static Dictionary<string, TEntity?> GroupByIdentifiers(IEnumerable<string> identifiers)
    {
        return identifiers
            .Distinct()
            .ToDictionary(id => id, _ => (TEntity?)default);
    }

    private static Dictionary<string, TEntity?> GroupByIdentifiers(IEnumerable<TEntity> entities)
    {
        var groups = entities.GroupBy(e => e.Id);

        var result = new Dictionary<string, TEntity?>();
        foreach (var group in groups)
            foreach (var element in group)
            {
                if (!result.TryAdd(group.Key, element))
                    throw new InvalidOperationException($"Dublicate entity with id = {group.Key}");
            }

        return result;
    }

    private static Dictionary<string, TEntity?> GroupByIdentifiers(IEnumerable<TEntity> entities, IEnumerable<string> identifiers)
    {
        var result = GroupByIdentifiers(entities);

        foreach (var id in identifiers)
            result.TryAdd(id, default);

        return result;
    }
}
