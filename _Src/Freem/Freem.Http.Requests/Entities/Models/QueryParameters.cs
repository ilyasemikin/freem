using System.Web;

namespace Freem.Http.Requests.Entities.Models;

public sealed class QueryParameters
{
    private readonly IDictionary<string, string> _collection;

    internal QueryParameters(string query)
    {
        var collection = HttpUtility.ParseQueryString(query);
        var dictionary = new Dictionary<string, string>();
        foreach (var key in collection.AllKeys)
        {
            if (key is null)
                continue;
            
            var value = collection[key];
            if (value is null)
                continue;
            
            dictionary.Add(key, value);
        }
        
        _collection = dictionary;
    }

    public QueryParameters With(string name, string value)
    {
        _collection.Add(name, value);
        return this;
    }

    public override string ToString()
    {
        if (_collection.Count == 0)
            return string.Empty;
        
        var groups = _collection.Select(p => CreateGroup(p.Key, p.Value));
        return "?" + string.Join("&", groups);

        static string CreateGroup(string key, string value)
        {
            key = HttpUtility.UrlEncode(key);
            value = HttpUtility.UrlEncode(value);
            
            return $"{key}={value}";
        }
    }
}