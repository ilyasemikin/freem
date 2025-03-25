using System.Net.Mime;

namespace Freem.Http.Requests.Entities.Models.Contents.Abstractions;

public interface IHttpResponseContent : IDisposable
{ 
    ContentType? ContentType { get; }

    Task<Stream> ReadAsStreamAsync(CancellationToken cancellationToken = default);
}