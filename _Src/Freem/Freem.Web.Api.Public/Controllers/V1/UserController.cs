using System.ComponentModel.DataAnnotations;
using Freem.Web.Api.Public.Contracts.Users.LoginPassword;
using Freem.Web.Api.Public.Contracts.Users.Settings;
using Freem.Web.Api.Public.Contracts.Users.Tokens;
using Freem.Web.Api.Public.Controllers.V1.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1;

[Route("api/v1/user")]
public sealed class UserController : BaseController
{
    [HttpPost("password-credentials/register")]
    public Task<ActionResult> RegisterAsync(
        [Required] [FromBody] RegisterPasswordCredentialsRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("password-credentials/login")]
    public Task<ActionResult<LoginPasswordCredentialsResponse>> LoginAsync(
        [Required] [FromBody] LoginPasswordCredentialsRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut("password-credentials")]
    public Task<ActionResult> UpdateAsync(
        [Required] [FromBody] UpdatePasswordCredentialsRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [HttpPost("tokens/refresh")]
    public Task<ActionResult<RefreshTokensResponse>> RefreshTokenAsync(
        [Required] [FromBody] RefreshTokensRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut("settings")]
    public Task<ActionResult> UpdateSettingsAsync(
        [Required] [FromBody] UpdateUserSettingsRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet("settings")]
    public Task<ActionResult<UserSettingsResponse>> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}