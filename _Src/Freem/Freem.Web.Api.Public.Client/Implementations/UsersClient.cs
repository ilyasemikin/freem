using System.Text.Json;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Users;
using Freem.Web.Api.Public.Contracts.DTO.Users.Settings;
using Utilities.Http;
using Utilities.Http.Extensions;

namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class UsersClient : BaseClient
{
    private readonly JsonSerializerOptions _options;
    
    public UsersClient(IHttpClient client, JsonSerializerOptions options, CurrentUser currentUser) 
        : base(client, options, currentUser)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public Task<ClientResult<RegisterUserResponse>> RegisterAsync(
        RegisterUserRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/user/register")
            .WithJsonBody(body, _options);

        return SendAsync<RegisterUserResponse>(request, cancellationToken);
    }

    public Task<ClientResult> UpdateSettingsAsync(
        UpdateUserSettingsRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put("api/v1/user/settings")
            .WithJsonBody(body, _options);
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<ClientResult<UserSettingsResponse>> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/user/settings");
        
        return SendForUserAsync<UserSettingsResponse>(request, cancellationToken);
    }

    public Task<ClientResult<MeResponse>> MeAsync(CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/user/me");
        
        return SendForUserAsync<MeResponse>(request, cancellationToken);
    }
}