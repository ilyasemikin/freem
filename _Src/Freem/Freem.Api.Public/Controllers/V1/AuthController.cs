using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Freem.Api.Public.Constants;
using Freem.Api.Public.Contracts.DTO;
using Freem.Api.Public.Contracts.DTO.Auth;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1;

[ApiController]
[AllowAnonymous]
[Route("api/v1/auth")]
[Tags(ControllerTags.Auth)]
[Produces(MediaTypeNames.Application.Json)]
public sealed class AuthController : ControllerBase
{
    private readonly IRequestMediator<ApiAnonymousHandlerContext> _mediator;

    public AuthController(IRequestMediator<ApiAnonymousHandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        _mediator = mediator;
    }

    [HttpPost("register")]
    [Consumes(MediaTypeNames.Application.Json)]
    [EndpointSummary("Register a password identity in EgoHub (does not start a session)")]
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

    [HttpPost("login")]
    [Consumes(MediaTypeNames.Application.Json)]
    [EndpointSummary("Log in and start a session; the refresh token is stored in an HTTP-only cookie")]
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

    [HttpPost("refresh")]
    [EndpointSummary("Rotate the session using the refresh token from the cookie")]
    [ProducesResponseType<SessionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> RefreshAsync(CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAnonymousHandlerContext();
        var request = ApiRefreshRequest.Instance;
        return await _mediator.ExecuteAsync<ApiRefreshRequest, IActionResult>(context, request, cancellationToken);
    }

    [HttpPost("logout")]
    [EndpointSummary("Revoke the session identified by the refresh token cookie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAnonymousHandlerContext();
        var request = ApiLogoutRequest.Instance;
        return await _mediator.ExecuteAsync<ApiLogoutRequest, IActionResult>(context, request, cancellationToken);
    }
}
