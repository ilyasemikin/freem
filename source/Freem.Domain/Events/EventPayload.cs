using System.Globalization;
using Utilities.Naming.Abstractions;

namespace Freem.Domain.Events;

public sealed class EventPayload
{
    private const char ListSeparator = ',';

    private readonly INamingPolicy _policy;
    private readonly Dictionary<string, string?> _data;

    public string? this[string key] => _data.GetValueOrDefault(key);

    public EventPayload(INamingPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(policy);

        _policy = policy;
        _data = [];
    }

    public EventPayload(IReadOnlyDictionary<string, string?> data, INamingPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentNullException.ThrowIfNull(policy);

        _policy = policy;
        _data = new Dictionary<string, string?>(data);
    }

    public EventPayload Set(string name, string? value)
    {
        var key = _policy.ConvertName(name);
        _data[key] = value;
        return this;
    }

    public EventPayload Set<T>(string name, T? value)
    {
        return Set(name, value?.ToString());
    }

    public EventPayload SetDate(string name, DateTimeOffset value)
    {
        var text = value.ToString("O", CultureInfo.InvariantCulture);
        return Set(name, text);
    }

    public EventPayload SetList<T>(string name, IEnumerable<T> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        var items = values.Select(value => value?.ToString());
        var text = string.Join(ListSeparator, items);
        return Set(name, text);
    }

    public string GetRequired(string name)
    {
        var key = _policy.ConvertName(name);
        if (!_data.TryGetValue(key, out var value) || value is null)
            throw new FormatException($"Missing required payload key [{key}]");

        return value;
    }

    public string? GetOptional(string name)
    {
        var key = _policy.ConvertName(name);
        return _data.GetValueOrDefault(key);
    }

    public T Get<T>(string name, Func<string, IFormatProvider?, T> parse)
    {
        var raw = GetRequired(name);
        return parse(raw, CultureInfo.InvariantCulture);
    }

    public T? GetOptional<T>(string name, Func<string, IFormatProvider?, T> parse)
        where T : class
    {
        var raw = GetOptional(name);
        return raw is null ? null : parse(raw, CultureInfo.InvariantCulture);
    }

    public DateTimeOffset GetDate(string name)
    {
        var raw = GetRequired(name);
        return DateTimeOffset.Parse(raw, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }

    public IReadOnlyList<T> GetList<T>(string name, Func<string, IFormatProvider?, T> parse)
    {
        var raw = GetRequired(name);
        if (raw.Length == 0)
            return [];

        return raw
            .Split(ListSeparator)
            .Select(item => parse(item, CultureInfo.InvariantCulture))
            .ToArray();
    }

    public IReadOnlyDictionary<string, string?> ToReadOnlyDictionary()
    {
        return new Dictionary<string, string?>(_data);
    }
}
