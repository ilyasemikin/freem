using System.Diagnostics.CodeAnalysis;

namespace Freem.Storage.Migrations.Constants.Collections;

public sealed class ConstantValuesCollection
{
    private readonly Dictionary<string, string> _values;

    public ConstantValuesCollection(IEnumerable<KeyValuePair<string, string>> pairs)
    {
        _values = pairs.ToDictionary(p => p.Key, p => p.Value);
    }

    public bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
    {
        return _values.TryGetValue(key, out value);
    }
}