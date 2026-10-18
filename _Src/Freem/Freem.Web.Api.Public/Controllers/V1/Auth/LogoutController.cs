using EgoHub.Api.Public.Client.Abstractions;
using EgoHub.Api.Public.Contracts.V1.Sessions;
using Freem.Web.Api.Public.Authentication.EgoHub;
using Freem.Web.Api.Public.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Auth;

[ApiController]
[AllowAnonymous]
[Route("api/v1/auth/logout")]
[Tags(ControllerTags.Auth)]
[ProducesResponseType(StatusCodes.Status200OK)]
public sealed class LogoutController : ControllerBase
{
    private readonly ISessionTokenClient _client;

    public LogoutController(ISessionTokenClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        _client = client;
    }

    [HttpPost]
    [EndpointSummary("Revoke the session identified by the refresh token cookie")]
    public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken = default)
    {
        Request.Cookies.TryGetValue(EgoHubAuthSupport.RefreshCookieName, out var refreshToken);
        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            var request = new RevokeSessionTokensRequest { RefreshToken = refreshToken };
            await _client.RevokeAsync(request, cancellationToken);
        }

        EgoHubAuthSupport.DeleteRefreshCookie(HttpContext);
        return Ok();
    }
}
