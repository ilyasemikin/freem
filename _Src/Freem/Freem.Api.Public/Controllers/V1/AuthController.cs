using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Freem.Api.Public.Contracts.V1;
using Freem.Api.Public.Contracts.V1.Auth;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1;

/// <summary>Sign-in flow. Proxies to the EgoHub identity service; the refresh token lives in an HTTP-only cookie. Anonymous.</summary>
[ApiController]
[AllowAnonymous]
[Route("api/v1/auth")]
[Produces(MediaTypeNames.Application.Json)]
public sealed class AuthController : ControllerBase
{
    private readonly IRequestMediator<ApiAnonymousHandlerContext> _mediator;

    public AuthController(IRequestMediator<ApiAnonymousHandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        _mediator = mediator;
    }

    /// <summary>Creates an identity in EgoHub. It does not create a Freem user (see <c>POST api/v1/user/register</c>) and does not start a session.</summary>
    /// <param name="body">Username and password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost("register")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType<RegisterResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> RegisterAsync(
        [Required] [FromBody] RegisterRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAnonymousHandlerContext();
        var request = new ApiRegisterRequest(body);
        return await _mediator.ExecuteAsync<ApiRegisterRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Authenticates the user and starts a session.</summary>
    /// <param name="body">Username and password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The access token and its expiration time; the refresh token is set as an HTTP-only cookie.</returns>
    [HttpPost("login")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType<SessionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> LoginAsync(
        [Required] [FromBody] LoginRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAnonymousHandlerContext();
        var request = new ApiLoginRequest(body);
        return await _mediator.ExecuteAsync<ApiLoginRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Issues a new access token and rotates the refresh token cookie.</summary>
    [HttpPost("refresh")]
    [ProducesResponseType<SessionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> RefreshAsync(CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAnonymousHandlerContext();
        var request = ApiRefreshRequest.Instance;
        return await _mediator.ExecuteAsync<ApiRefreshRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Ends the session and revokes its refresh token.</summary>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAnonymousHandlerContext();
        var request = ApiLogoutRequest.Instance;
        return await _mediator.ExecuteAsync<ApiLogoutRequest, IActionResult>(context, request, cancellationToken);
    }
}
