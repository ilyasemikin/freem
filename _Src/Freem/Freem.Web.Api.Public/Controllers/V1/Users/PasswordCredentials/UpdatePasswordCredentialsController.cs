using System.ComponentModel.DataAnnotations;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Password.Update;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Autherization;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Users.LoginPassword;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Users.PasswordCredentials;

[Authorize(JwtAuthorizationPolicy.Name)]
[Route("api/v1/user/password-credentials")]
[Tags(ControllerTags.User, ControllerTags.PasswordCredentials)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class UpdatePasswordCredentialsController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public UpdatePasswordCredentialsController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPut]
    [EndpointSummary("Update password credentials or set if not exists")]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromBody] UpdatePasswordCredentialsRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static UpdateLoginCredentialsRequest Map(UpdatePasswordCredentialsRequest request)
    {
        return new UpdateLoginCredentialsRequest(request.OldPassword, request.NewPassword);
    }

    private static IActionResult CreateFailure(Error<UpdateLoginCredentialsErrorCode> error)
    {
        return error.Code switch
        {
            UpdateLoginCredentialsErrorCode.UserNotFound => new UnauthorizedResult(),
            UpdateLoginCredentialsErrorCode.PasswordCredentialsNotAllowed => new UnprocessableEntityResult(),
            UpdateLoginCredentialsErrorCode.InvalidCredentials => new ForbidResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}