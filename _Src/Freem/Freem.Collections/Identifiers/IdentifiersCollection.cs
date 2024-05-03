using System.Collections;
using Freem.Collections.Identifiers.Abstractions;

namespace Freem.Collections.Identifiers;

public sealed class IdentifiersCollection : IIdentifiersCollection, IReadOnlyCollection<string>
{
    private readonly HashSet<string> _identifiers;

    public int Count => _identifiers.Count;
    public bool Empty => Count == 0;

    public IdentifiersCollection(IEnumerable<string>? identifiers)
    {
        _identifiers = (identifiers ?? []).ToHashSet();

        foreach (var id in _identifiers)
            ArgumentException.ThrowIfNullOrWhiteSpace(id);
    }

    public bool Contains(string identifier)
    {
        return _identifiers.Contains(identifier);
    }

    public void Add(string identifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);

        _identifiers.Add(identifier);
    }

    public void AddRange(IEnumerable<string> identifiers)
    {
        ArgumentNullException.ThrowIfNull(identifiers);

        foreach (var id in identifiers)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(id);

            _identifiers.Add(id);
        }
    }

    public void Remove(string identifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);

        _identifiers.Remove(identifier);
    }

    public void Update(IEnumerable<string> identifiers)
    {
        ArgumentNullException.ThrowIfNull(identifiers);

        _identifiers.Clear();

        foreach (var id in identifiers)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(id);

            _identifiers.Add(id);
        }
    }

    public void Clear()
    {
        _identifiers.Clear();
    }

    public IEnumerator<string> GetEnumerator()
    {
        return _identifiers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
