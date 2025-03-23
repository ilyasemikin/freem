using System.Net.Mime;
using Freem.Http.Requests.Entities.Models.Contents.Abstractions;

namespace Freem.Http.Requests.Entities.Models.Contents;

public sealed class HttpResponseContent : IHttpResponseContent
{
    private readonly HttpContent _content;
    private bool _disposed;
    
    public ContentType? ContentType { get; }
    
    public HttpResponseContent(HttpContent content)
    {
        ArgumentNullException.ThrowIfNull(content);
        
        _content = content;
        _disposed = false;

        if (_content.Headers.ContentType?.MediaType is null)
            return;
        
        var mt = _content.Headers.ContentType.MediaType;
        var cs = _content.Headers.ContentType.CharSet;
        
        ContentType = new ContentType()
        {
            MediaType = mt,
            CharSet = cs
        };
    }

    public async Task<Stream> ReadAsStreamAsync(CancellationToken cancellationToken = default)
    {
        if (_disposed)
            throw new InvalidOperationException("Already disposed");
        
        var stream = await _content.ReadAsStreamAsync(cancellationToken);
        stream.Position = 0;

        return stream;
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        
        _content.Dispose();
        _disposed = true;
    }
}