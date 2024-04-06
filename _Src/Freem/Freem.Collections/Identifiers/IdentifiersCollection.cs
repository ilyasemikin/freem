using System.Collections;
using Freem.Collections.Identifiers.Abstractions;
using Freem.Collections.Identifiers.Validation.Abstractions;

namespace Freem.Collections.Identifiers;

public sealed class IdentifiersCollection : IIdentifiersCollection, IReadOnlyCollection<string>
{
    private readonly IIdentifierActionCheckerStrategy _actionChecker;
    private readonly HashSet<string> _identifiers;

    public int Count => _identifiers.Count;
    public bool Empty => Count == 0;

    public IdentifiersCollection(IEnumerable<string>? identifiers, IIdentifierActionCheckerStrategy? actionValidator = null)
    {
        _actionChecker = actionValidator ?? IIdentifierActionCheckerStrategy.AllAllowedChecker;
        _identifiers = (identifiers ?? []).ToHashSet();

        foreach (var id in _identifiers)
            ArgumentException.ThrowIfNullOrWhiteSpace(id);

        _actionChecker.CheckUpdate(this, _identifiers);
    }

    public bool Contains(string identifier)
    {
        return _identifiers.Contains(identifier);
    }

    public void Add(string identifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);

        if (!Contains(identifier))
            return;

        _actionChecker.CheckAdd(this, identifier);
        _identifiers.Add(identifier);
    }

    public void AddRange(IEnumerable<string> identifiers)
    {
        ArgumentNullException.ThrowIfNull(identifiers);

        var set = identifiers
            .Except(_identifiers)
            .ToHashSet();

        if (set.Count == 0)
            return;

        foreach (var id in set)
            ArgumentException.ThrowIfNullOrWhiteSpace(id);

        _actionChecker.CheckAddRange(this, set);
        
        foreach (var id in set)
            _identifiers.Add(id);
    }

    public void Remove(string identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        if (!Contains(identifier))
            return;

        _actionChecker.CheckRemove(this, identifier);
        _identifiers.Remove(identifier);
    }

    public void Update(IEnumerable<string> identifiers)
    {
        ArgumentNullException.ThrowIfNull(identifiers);

        var set = identifiers
            .Except(_identifiers)
            .ToHashSet();

        foreach (var id in set)
            ArgumentException.ThrowIfNullOrWhiteSpace(id);

        _actionChecker.CheckUpdate(this, set);

        foreach (var id in set)
            _identifiers.Add(id);
    }

    public void Clear()
    {
        if (Empty)
            return;

        _actionChecker?.CheckClear(this);
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
