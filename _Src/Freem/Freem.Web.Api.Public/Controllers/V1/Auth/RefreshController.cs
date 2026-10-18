using EgoHub.Api.Public.Client.Abstractions;
using EgoHub.Api.Public.Contracts.V1.Sessions;
using Freem.Web.Api.Public.Authentication.EgoHub;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Auth;

[ApiController]
[AllowAnonymous]
[Route("api/v1/auth/refresh")]
[Tags(ControllerTags.Auth)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status502BadGateway)]
public sealed class RefreshController : ControllerBase
{
    private readonly ISessionTokenClient _client;

    public RefreshController(ISessionTokenClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        _client = client;
    }

    [HttpPost]
    [EndpointSummary("Rotate the session using the refresh token from the cookie")]
    public async Task<IActionResult> RefreshAsync(CancellationToken cancellationToken = default)
    {
        Request.Cookies.TryGetValue(EgoHubAuthSupport.RefreshCookieName, out var refreshToken);
        if (string.IsNullOrWhiteSpace(refreshToken))
            return StatusCode(StatusCodes.Status401Unauthorized);

        var request = new RefreshSessionTokensRequest { RefreshToken = refreshToken };

        var result = await _client.RefreshAsync(request, cancellationToken);
        if (!result.Success)
        {
            if (EgoHubAuthSupport.IsRejection(result.Error))
                EgoHubAuthSupport.DeleteRefreshCookie(HttpContext);

            return EgoHubAuthSupport.MapError(result.Error);
        }

        var tokens = result.Value.Tokens;
        EgoHubAuthSupport.AppendRefreshCookie(HttpContext, tokens);

        var response = EgoHubAuthSupport.ToSessionResponse(tokens);
        return Ok(response);
    }
}
