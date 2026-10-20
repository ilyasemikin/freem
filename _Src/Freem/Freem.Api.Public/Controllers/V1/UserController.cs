using System.ComponentModel.DataAnnotations;
using Freem.Api.Public.Constants;
using Freem.Api.Public.Contracts.DTO;
using Freem.Api.Public.Contracts.DTO.Users;
using Freem.Api.Public.Contracts.DTO.Users.Settings;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Users;
using Freem.Api.Public.UseCases.Contracts.Users.Settings;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1;

[Route("api/v1/user")]
public sealed class UserController : BaseController
{
    public UserController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    [HttpGet("me")]
    [Tags(ControllerTags.User, ControllerTags.Settings)]
    [EndpointSummary("Get information about me")]
    [ProducesResponseType<MeResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = ApiGetUserRequest.Instance;
        return await Mediator.ExecuteAsync<ApiGetUserRequest, IActionResult>(context, request, cancellationToken);
    }

    [HttpPost("register")]
    [Tags(ControllerTags.User)]
    [EndpointSummary("Register user")]
    [ProducesResponseType<RegisterUserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterAsync(
        [Required] [FromBody] RegisterUserRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiRegisterUserRequest(body);
        return await Mediator.ExecuteAsync<ApiRegisterUserRequest, IActionResult>(context, request, cancellationToken);
    }

    [HttpGet("settings")]
    [Tags(ControllerTags.User, ControllerTags.Settings)]
    [EndpointSummary("Get user settings")]
    [ProducesResponseType<UserSettingsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = ApiGetUserSettingsRequest.Instance;
        return await Mediator.ExecuteAsync<ApiGetUserSettingsRequest, IActionResult>(context, request, cancellationToken);
    }

    [HttpPut("settings")]
    [Tags(ControllerTags.User, ControllerTags.Settings)]
    [EndpointSummary("Update user settings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateSettingsAsync(
        [Required] [FromBody] UpdateUserSettingsRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiUpdateUserSettingsRequest(body);
        return await Mediator.ExecuteAsync<ApiUpdateUserSettingsRequest, IActionResult>(context, request, cancellationToken);
    }
}
