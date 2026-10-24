using System.Text.Json;
using Freem.Api.Public.Client.Implementations.Base;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations;

public sealed class HealthClient : BaseClient
{
    public HealthClient(IHttpClient client, JsonSerializerOptions options)
        : base(client, options)
    {
    }

    public Task<Result<IClientError>> GetLivenessAsync(CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("_health/liveness");

        return SendAsync(request, cancellationToken);
    }
}
