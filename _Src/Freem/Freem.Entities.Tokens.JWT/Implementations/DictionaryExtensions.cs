using System.Diagnostics.CodeAnalysis;

namespace Freem.Entities.Tokens.JWT.Implementations;

internal static class DictionaryExtensions
{
    public static bool TryGet<T>(
        this IDictionary<string, object> dictionary, 
        string key,
        [NotNullWhen(true)] out T? result)
    {
        result = default;
        if (dictionary.TryGetValue(key, out var value) && value is T typedValue)
            result = typedValue;

        return result is not null;
    }
}