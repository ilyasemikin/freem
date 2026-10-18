using System.ComponentModel.DataAnnotations;
using EgoHub.Api.Public.Client.Abstractions;
using EgoHub.Api.Public.Contracts.V1.Identities;
using Freem.Web.Api.Public.Authentication.EgoHub;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Auth;

[AllowAnonymous]
[Route("api/v1/auth/login")]
[Tags(ControllerTags.Auth)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status502BadGateway)]
public sealed class LoginController : BaseController
{
    private readonly IPasswordIdentityClient _client;

    public LoginController(IPasswordIdentityClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        _client = client;
    }

    [HttpPost]
    [EndpointSummary("Log in and start a session; the refresh token is stored in an HTTP-only cookie")]
    public async Task<IActionResult> LoginAsync(
        [Required] [FromBody] LoginRequest body,
        CancellationToken cancellationToken = default)
    {
        var request = new LoginPasswordIdentityRequest
        {
            Username = body.Username,
            Password = body.Password
        };

        var result = await _client.LoginAsync(request, cancellationToken);
        if (!result.Success)
            return EgoHubAuthSupport.MapError(result.Error);

        var tokens = result.Value.Tokens;
        EgoHubAuthSupport.AppendRefreshCookie(HttpContext, tokens);

        var response = EgoHubAuthSupport.ToSessionResponse(tokens);
        return Ok(response);
    }
}
