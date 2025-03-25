using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Http.Requests.Entities.Models.Contents;
using Freem.Http.Requests.Entities.Models.Contents.Abstractions;
using NetHttpClient = System.Net.Http.HttpClient;

namespace Freem.Http.Requests.Implementations;

public sealed class HttpClient : IHttpClient
{
    private readonly NetHttpClient _client;

    private readonly string? _baseUri;
    
    public HttpClient(NetHttpClient client, string? baseUri = null)
    {
        _baseUri = baseUri;
        if (_baseUri is not null && !_baseUri.EndsWith('/'))
            _baseUri += '/';

        _client = client;
    }

    public async Task<HttpResponse> SendAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        var uri = request.BuildUri();
        if (!string.IsNullOrWhiteSpace(_baseUri))
            uri = new Uri(_baseUri + uri);

        var content = request.Content is not null
            ? new ClientContent(request.Content)
            : null;
        
        var message = new HttpRequestMessage(request.Method, uri)
        {
            Content = content
        };
        
        foreach (var (name, value) in request.Headers.Get())
            message.Headers.Add(name, value);

        var response = await _client.SendAsync(message, cancellationToken);
        var rc = new HttpResponseContent(response.Content);
        
        return new HttpResponse(response.StatusCode, rc);
    }

    private sealed class ClientContent : HttpContent
    {
        private readonly IHttpRequestContent _content;

        public ClientContent(IHttpRequestContent content)
        {
            ArgumentNullException.ThrowIfNull(content);
            
            _content = content;
            Headers.ContentType = Map(_content.ContentType);
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            return _content.SerializeToStreamAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }

        private static MediaTypeHeaderValue Map(ContentType type)
        {
            var mt = type.MediaType;
            var cs = type.CharSet;
            return new MediaTypeHeaderValue(mt, cs);
        }
    }
}