using System.Net.Mime;

namespace Freem.Http.Requests.Entities.Models.Contents.Abstractions;

public interface IHttpRequestContent
{
    ContentType ContentType { get; }
    
    Task SerializeToStreamAsync(Stream stream, CancellationToken cancellationToken = default);
}