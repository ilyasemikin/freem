using Freem.Http.Requests.Entities.Models;
using Freem.Http.Requests.Entities.Models.Contents.Abstractions;

namespace Freem.Http.Requests.Entities;

public sealed class HttpRequest
{
    public HttpMethod Method { get; }
    
    public Uri Path { get; }
    public QueryParameters Query { get; }
    
    public HttpRequestHeaders Headers { get; }
    
    public IHttpRequestContent? Content { get; }
    
    private HttpRequest(HttpMethod method, Uri path)
    {
        Method = method;

        var @string = path.ToString();
        var question = @string.IndexOf('?');

        var rp = question == -1 ? @string : @string[..question];
        var rq = question == -1 ? string.Empty : @string[(question + 1)..];

        Path = new Uri(rp, UriKind.RelativeOrAbsolute);
        Query = new QueryParameters(rq);
        
        Headers = new HttpRequestHeaders();
    }

    private HttpRequest(
        HttpMethod method, 
        Uri path, 
        QueryParameters query, 
        HttpRequestHeaders headers, 
        IHttpRequestContent? content)
    {
        Method = method;
        Path = path;
        Query = query;
        Headers = headers;
        Content = content;
    }

    public HttpRequest WithQueryParameter(string name, string value)
    {
        var query = Query.With(name, value);
        return new HttpRequest(Method, Path, query, Headers, Content);
    }

    public HttpRequest WithHeader(string name, string value)
    {
        var headers = Headers.WithHeader(name, value);
        return new HttpRequest(Method, Path, Query, headers, Content);
    }

    public HttpRequest WithHeader<T>(string name, T value)
        where T : struct, Enum
    {
        var headers = Headers.WithHeader(name, value);
        return new HttpRequest(Method, Path, Query, headers, Content);
    }
    
    public HttpRequest WithBody(IHttpRequestContent body)
    {
        return new HttpRequest(Method, Path, Query, Headers, body);
    }

    internal Uri BuildUri()
    {
        var query = Query.ToString();
        return query.Length == 0
            ? Path
            : new Uri(Path + query, UriKind.RelativeOrAbsolute);
    }

    public static HttpRequest Post(Uri path)
    {
        return new HttpRequest(HttpMethod.Post, path);
    }

    public static HttpRequest Post(string path)
    {
        var uri = new Uri(path, UriKind.RelativeOrAbsolute);
        return Post(uri);
    }

    public static HttpRequest Put(Uri path)
    {
        return new HttpRequest(HttpMethod.Put, path);
    }
    
    public static HttpRequest Put(string path)
    {
        var uri = new Uri(path, UriKind.RelativeOrAbsolute);
        return Put(uri);
    }

    public static HttpRequest Patch(Uri path)
    {
        return new HttpRequest(HttpMethod.Patch, path);
    }
    
    public static HttpRequest Patch(string path)
    {
        var uri = new Uri(path, UriKind.RelativeOrAbsolute);
        return Patch(uri);
    }

    public static HttpRequest Delete(Uri path)
    {
        return new HttpRequest(HttpMethod.Delete, path);
    }
    
    public static HttpRequest Delete(string path)
    {
        var uri = new Uri(path, UriKind.RelativeOrAbsolute);
        return Delete(uri);
    }

    public static HttpRequest Get(Uri path)
    {
        return new HttpRequest(HttpMethod.Get, path);
    }
    
    public static HttpRequest Get(string path)
    {
        var uri = new Uri(path, UriKind.RelativeOrAbsolute);
        return Get(uri);
    }
}
