using System.Net;
using Freem.Enums.Exceptions;
using Freem.Http.Requests.Entities.Models.Contents.Abstractions;

namespace Freem.Http.Requests.Entities;

public sealed class HttpResponse : IDisposable
{
    private bool _disposed;
    
    public HttpStatusCode StatusCode { get; }
    public IHttpResponseContent Content { get; }
    
    public bool Success => (int)StatusCode >= 200 && (int)StatusCode <= 299; 

    internal HttpResponse(HttpStatusCode statusCode, IHttpResponseContent content)
    {
        InvalidEnumValueException<HttpStatusCode>.ThrowIfValueInvalid(statusCode);
        
        StatusCode = statusCode;
        Content = content;
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        
        Content.Dispose();
        _disposed = true;
    }
}