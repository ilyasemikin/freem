using System.Text.Json;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Api.Public.Contracts.V1.Users;
using Freem.Api.Public.Contracts.V1.Users.Settings;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Http.Extensions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations;

public sealed class UsersClient : BaseClient
{
    private readonly JsonSerializerOptions _options;
    
    public UsersClient(IHttpClient client, JsonSerializerOptions options, ClientSession session) 
        : base(client, options, session)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public Task<Result<UserRegisterResponse, IClientError>> RegisterAsync(
        UserRegisterRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/user/register")
            .WithJsonBody(body, _options);

        return SendForUserAsync<UserRegisterResponse>(request, cancellationToken);
    }

    public Task<Result<IClientError>> UpdateSettingsAsync(
        UserSettingsUpdateRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put("api/v1/user/settings")
            .WithJsonBody(body, _options);
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<UserSettingsResponse, IClientError>> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/user/settings");
        
        return SendForUserAsync<UserSettingsResponse>(request, cancellationToken);
    }

    public Task<Result<UserResponse, IClientError>> MeAsync(CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/user/me");
        
        return SendForUserAsync<UserResponse>(request, cancellationToken);
    }
}