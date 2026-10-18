using System.Text.Json;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Users;
using Freem.Web.Api.Public.Contracts.DTO.Users.Settings;
using Freem.Web.Api.Public.SyncClient.Implementations.Base;
using Utilities.Http;
using Utilities.Http.Extensions;

namespace Freem.Web.Api.Public.SyncClient.Implementations;

public sealed class UsersSyncClient : BaseSyncClient
{
    private readonly JsonSerializerOptions _options;
    
    public UsersSyncClient(IHttpClient client, JsonSerializerOptions options, CurrentUser currentUser) 
        : base(client, options, currentUser)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }
    
    public ClientResult<RegisterUserResponse> Register(RegisterUserRequest body)
    {
        var request = HttpRequest.Post("api/v1/user/register")
            .WithJsonBody(body, _options);

        return Send<RegisterUserResponse>(request);
    }

    public ClientResult UpdateSettings(UpdateUserSettingsRequest body)
    {
        var request = HttpRequest.Put("api/v1/user/settings")
            .WithJsonBody(body, _options);
        
        return SendForUser(request);
    }

    public ClientResult<UserSettingsResponse> GetSettings()
    {
        var request = HttpRequest.Get("api/v1/user/settings");

        return SendForUser<UserSettingsResponse>(request);
    }

    public ClientResult<MeResponse> Me()
    {
        var request = HttpRequest.Get("api/v1/user/me");
        
        return SendForUser<MeResponse>(request);
    }
}