using System.ComponentModel.DataAnnotations;
using EgoHub.Api.Public.Client.Abstractions;
using EgoHub.Api.Public.Contracts.V1.Identities;
using Freem.Entities.Users.Identifiers;
using Freem.Web.Api.Public.Authentication.EgoHub;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Auth;

[AllowAnonymous]
[Route("api/v1/auth/register")]
[Tags(ControllerTags.Auth)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponse))]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[ProducesResponseType(StatusCodes.Status502BadGateway)]
public sealed class RegisterController : BaseController
{
    private readonly IPasswordIdentityClient _client;

    public RegisterController(IPasswordIdentityClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        _client = client;
    }

    [HttpPost]
    [EndpointSummary("Register a password identity in EgoHub (does not start a session)")]
    public async Task<IActionResult> RegisterAsync(
        [Required] [FromBody] RegisterRequest body,
        CancellationToken cancellationToken = default)
    {
        var request = new RegisterPasswordIdentityRequest
        {
            Username = body.Username,
            Password = body.Password
        };

        var result = await _client.RegisterAsync(request, cancellationToken);
        if (!result.Success)
            return EgoHubAuthSupport.MapError(result.Error);

        var userIdValue = result.Value.UserId.ToString();
        var userId = new UserIdentifier(userIdValue);
        var response = new RegisterResponse(userId);
        return Ok(response);
    }
}
