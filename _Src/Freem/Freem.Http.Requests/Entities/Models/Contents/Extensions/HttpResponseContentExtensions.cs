using System.Text.Json;
using Freem.Http.Requests.Entities.Models.Contents.Abstractions;

namespace Freem.Http.Requests.Entities.Models.Contents.Extensions;

public static class HttpResponseContentExtensions
{
    public static async Task<T> ReadJsonAsAsync<T>(this IHttpResponseContent content, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        await using var stream = await content.ReadAsStreamAsync(cancellationToken);
        
        var value = await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);
        if (value is null)
            throw new InvalidOperationException();
        
        return value;
    }
}