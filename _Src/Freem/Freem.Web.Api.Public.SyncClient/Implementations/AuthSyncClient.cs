using System.Text.Json;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Auth;
using Freem.Web.Api.Public.SyncClient.Implementations.Base;
using Utilities.Http;
using Utilities.Http.Extensions;

namespace Freem.Web.Api.Public.SyncClient.Implementations;

public sealed class AuthSyncClient : BaseSyncClient
{
    private readonly JsonSerializerOptions _options;

    public AuthSyncClient(IHttpClient client, JsonSerializerOptions options)
        : base(client, options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options;
    }

    public ClientResult<RegisterResponse> Register(RegisterRequest body)
    {
        var request = HttpRequest.Post("api/v1/auth/register")
            .WithJsonBody(body, _options);

        return Send<RegisterResponse>(request);
    }

    public ClientResult<SessionResponse> Login(LoginRequest body)
    {
        var request = HttpRequest.Post("api/v1/auth/login")
            .WithJsonBody(body, _options);

        return Send<SessionResponse>(request);
    }
}
