using Freem.Enums.Exceptions;

namespace Freem.Http.Requests.Entities.Models;

public sealed class HttpRequestHeaders
{
    private readonly IDictionary<string, string> _collection;

    public HttpRequestHeaders()
    {
        _collection = new Dictionary<string, string>();
    }

    public HttpRequestHeaders WithHeader(string key, string value)
    {
        _collection.Add(key, value);
        return this;
    }

    public HttpRequestHeaders WithHeader<T>(string key, T value)
        where T : struct, Enum
    {
        InvalidEnumValueException<T>.ThrowIfValueInvalid(value);

        var @string = value.ToString("G");
        return WithHeader(key, @string);
    }

    internal IEnumerable<KeyValuePair<string, string>> Get()
    {
        return _collection;
    }
}