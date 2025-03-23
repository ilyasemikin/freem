using System.Text.Json;
using Freem.Http.Requests.Entities.Models.Contents;

namespace Freem.Http.Requests.Entities.Extensions;

public static class RequestExtensions
{
    public static HttpRequest WithQueryParameterIfNotNull(this HttpRequest request, string name, string? value)
    {
        return value is not null
            ? request.WithQueryParameter(name, value)
            : request;
    }
    
    public static HttpRequest WithJsonBody<T>(this HttpRequest request, T value, JsonSerializerOptions? options = null)
        where T : class
    {
        var content = new JsonHttpRequestContent(value, options);
        return request.WithBody(content);
    }
}