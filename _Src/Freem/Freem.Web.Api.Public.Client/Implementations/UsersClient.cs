using System.Text.Json;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Http.Requests.Entities.Extensions;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.Users.LoginPassword;
using Freem.Web.Api.Public.Contracts.Users.Settings;
using Freem.Web.Api.Public.Contracts.Users.Tokens;

namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class UsersClient : BaseClient
{
    private readonly JsonSerializerOptions _options;
    
    public UsersClient(IHttpClient client, JsonSerializerOptions options, TokenLoader tokenLoader) 
        : base(client, options, tokenLoader)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public Task<ClientResult> RegisterAsync(
        RegisterPasswordCredentialsRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/user/password-credentials/register")
            .WithJsonBody(body, _options);

        return SendAsync(request, cancellationToken);
    }

    public Task<ClientResult<LoginPasswordCredentialsResponse>> LoginAsync(
        LoginPasswordCredentialsRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/user/password-credentials/login")
            .WithJsonBody(body, _options);
        
        return SendAsync<LoginPasswordCredentialsResponse>(request, cancellationToken);
    }

    public Task<ClientResult> UpdatePasswordCredentialsAsync(
        UpdatePasswordCredentialsRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put("api/v1/user/password-credentials")
            .WithJsonBody(body, _options);
        
        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult> UpdateSettingsAsync(
        UpdateUserSettingsRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put("api/v1/user/settings")
            .WithJsonBody(body, _options);
        
        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult<UserSettingsResponse>> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/user/settings");
        
        return SendAuthorizedAsync<UserSettingsResponse>(request, cancellationToken);
    }

    public Task<ClientResult<RefreshTokensResponse>> RefreshTokensAsync(
        RefreshTokensRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/user/tokens/refresh")
            .WithJsonBody(body, _options);
        
        return SendAsync<RefreshTokensResponse>(request, cancellationToken);
    }
}