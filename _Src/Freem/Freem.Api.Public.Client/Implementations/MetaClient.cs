using System.Text.Json;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Api.Public.Contracts;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations;

public sealed class MetaClient : BaseClient
{
    public MetaClient(IHttpClient client, JsonSerializerOptions options)
        : base(client, options)
    {
    }

    public Task<Result<VersionResponse, IClientError>> GetVersionAsync(CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("_meta/version");

        return SendAsync<VersionResponse>(request, cancellationToken);
    }
}
