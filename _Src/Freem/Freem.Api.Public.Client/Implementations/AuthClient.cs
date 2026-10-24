using System.Text.Json;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Api.Public.Contracts.V1.Auth;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Http.Extensions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations;

public sealed class AuthClient : BaseClient
{
    private readonly JsonSerializerOptions _options;

    public AuthClient(IHttpClient client, JsonSerializerOptions options)
        : base(client, options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options;
    }

    public Task<Result<RegisterResponse, IClientError>> RegisterAsync(
        RegisterRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/auth/register")
            .WithJsonBody(body, _options);

        return SendAsync<RegisterResponse>(request, cancellationToken);
    }

    public Task<Result<SessionResponse, IClientError>> LoginAsync(
        LoginRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/auth/login")
            .WithJsonBody(body, _options);

        return SendAsync<SessionResponse>(request, cancellationToken);
    }
}
