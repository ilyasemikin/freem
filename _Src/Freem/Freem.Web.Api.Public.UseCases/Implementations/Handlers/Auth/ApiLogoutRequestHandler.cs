using EgoHub.Api.Public.Client.Abstractions;
using EgoHub.Api.Public.Contracts.V1.Sessions;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.UseCases.Implementations.Handlers.Auth;

internal sealed class ApiLogoutRequestHandler : BaseAnonymousRequestHandler<ApiLogoutRequest>
{
    private readonly ISessionTokenClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiLogoutRequestHandler(ISessionTokenClient client, IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);

        _client = client;
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAnonymousHandlerContext context, ApiLogoutRequest request,
        CancellationToken cancellationToken = default)
    {
        var httpContext = _httpContextAccessor.HttpContext!;

        httpContext.Request.Cookies.TryGetValue(EgoHubAuthSupport.RefreshCookieName, out var refreshToken);
        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            var egoHubRequest = new RevokeSessionTokensRequest { RefreshToken = refreshToken };
            await _client.RevokeAsync(egoHubRequest, cancellationToken);
        }

        EgoHubAuthSupport.DeleteRefreshCookie(httpContext);
        return new OkResult();
    }
}
