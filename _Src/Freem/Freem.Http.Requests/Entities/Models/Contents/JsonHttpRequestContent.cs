using System.Net.Mime;
using System.Text.Json;
using Freem.Http.Requests.Entities.Models.Contents.Abstractions;

namespace Freem.Http.Requests.Entities.Models.Contents;

public sealed class JsonHttpRequestContent : IHttpRequestContent
{
    private readonly object _value;
    private readonly JsonSerializerOptions? _options;

    public ContentType ContentType { get; }

    public JsonHttpRequestContent(object value, JsonSerializerOptions? options)
    {
        _value = value;
        _options = options;
        
        ContentType = new ContentType(MediaTypeNames.Application.Json);
    }
    
    public Task SerializeToStreamAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        return JsonSerializer.SerializeAsync(stream, _value, _options, cancellationToken);
    }
}