using System.Text.Json;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Http.Requests.Entities.Extensions;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Users;
using Freem.Web.Api.Public.Contracts.DTO.Users.LoginPassword;
using Freem.Web.Api.Public.Contracts.DTO.Users.Settings;
using Freem.Web.Api.Public.Contracts.DTO.Users.Tokens;
using Freem.Web.Api.Public.SyncClient.Implementations.Base;

namespace Freem.Web.Api.Public.SyncClient.Implementations;

public sealed class UsersSyncClient : BaseSyncClient
{
    private readonly JsonSerializerOptions _options;
    
    public UsersSyncClient(IHttpClient client, JsonSerializerOptions options, TokenLoader tokenLoader) 
        : base(client, options, tokenLoader)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }
    
    public ClientResult Register(RegisterPasswordCredentialsRequest body)
    {
        var request = HttpRequest.Post("api/v1/user/password-credentials/register")
            .WithJsonBody(body, _options);

        return Send(request);
    }

    public ClientResult<LoginPasswordCredentialsResponse> Login(LoginPasswordCredentialsRequest body)
    {
        var request = HttpRequest.Post("api/v1/user/password-credentials/login")
            .WithJsonBody(body, _options);

        return Send<LoginPasswordCredentialsResponse>(request);
    }

    public ClientResult UpdatePasswordCredentials(UpdatePasswordCredentialsRequest body)
    {
        var request = HttpRequest.Put("api/v1/user/password-credentials")
            .WithJsonBody(body, _options);

        return SendAuthorized(request);
    }

    public ClientResult UpdateSettings(UpdateUserSettingsRequest body)
    {
        var request = HttpRequest.Put("api/v1/user/settings")
            .WithJsonBody(body, _options);
        
        return SendAuthorized(request);
    }

    public ClientResult<UserSettingsResponse> GetSettings()
    {
        var request = HttpRequest.Get("api/v1/user/settings");

        return SendAuthorized<UserSettingsResponse>(request);
    }

    public ClientResult<RefreshTokensResponse> RefreshTokens(RefreshTokensRequest body)
    {
        var request = HttpRequest.Post("api/v1/user/tokens/refresh")
            .WithJsonBody(body, _options);

        return Send<RefreshTokensResponse>(request);
    }
    
    public ClientResult<MeResponse> Me()
    {
        var request = HttpRequest.Get("api/v1/user/me");
        
        return Send<MeResponse>(request);
    }
}