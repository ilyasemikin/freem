using EgoHub.Api.Public.Client.Abstractions;
using EgoHub.Api.Public.Contracts.V1.Identities;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Auth;

internal sealed class ApiLoginRequestHandler : BaseAnonymousRequestHandler<ApiLoginRequest>
{
    private readonly IPasswordIdentityClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiLoginRequestHandler(IPasswordIdentityClient client, IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);

        _client = client;
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAnonymousHandlerContext context, ApiLoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var egoHubRequest = new LoginPasswordIdentityRequest
        {
            Username = request.Body.Username,
            Password = request.Body.Password
        };

        var result = await _client.LoginAsync(egoHubRequest, cancellationToken);
        if (!result.Success)
            return EgoHubAuthSupport.MapError(result.Error);

        var tokens = result.Value.Tokens;
        EgoHubAuthSupport.AppendRefreshCookie(_httpContextAccessor.HttpContext!, tokens);

        var response = EgoHubAuthSupport.ToSessionResponse(tokens);
        return new OkObjectResult(response);
    }
}
