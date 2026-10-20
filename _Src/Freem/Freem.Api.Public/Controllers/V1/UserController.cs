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

/// <summary>The current user's profile and settings.</summary>
[Route("api/v1/user")]
public sealed class UserController : BaseController
{
    public UserController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    /// <summary>Returns the profile of the current user.</summary>
    [HttpGet("me")]
    [Tags(ControllerTags.User, ControllerTags.Settings)]
    [ProducesResponseType<MeResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = ApiGetUserRequest.Instance;
        return await Mediator.ExecuteAsync<ApiGetUserRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Creates the Freem user for an already existing EgoHub identity. The user id is taken from the access token.</summary>
    /// <param name="body">Nickname.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost("register")]
    [Tags(ControllerTags.User)]
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

    /// <summary>Returns the settings of the current user.</summary>
    [HttpGet("settings")]
    [Tags(ControllerTags.User, ControllerTags.Settings)]
    [ProducesResponseType<UserSettingsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = ApiGetUserSettingsRequest.Instance;
        return await Mediator.ExecuteAsync<ApiGetUserSettingsRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Partially updates the settings: only the fields present in the body are changed.</summary>
    /// <param name="body">Fields to change.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPut("settings")]
    [Tags(ControllerTags.User, ControllerTags.Settings)]
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
