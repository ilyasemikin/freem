using Freem.Http.Requests.Entities;

namespace Freem.Http.Requests.Abstractions;

public interface IHttpClient
{
    Task<HttpResponse> SendAsync(HttpRequest request, CancellationToken cancellationToken = default);
}