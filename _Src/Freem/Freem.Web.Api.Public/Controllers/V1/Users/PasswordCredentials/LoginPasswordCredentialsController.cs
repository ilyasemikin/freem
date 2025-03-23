using System.ComponentModel.DataAnnotations;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Password.Login;
using Freem.Entities.Users;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts.Users.LoginPassword;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Users.PasswordCredentials;

[Route("api/v1/user/password-credentials/login")]
public sealed class LoginPasswordCredentialsController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public LoginPasswordCredentialsController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginPasswordCredentialsResponse))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginAsync(
        [Required] [FromBody] LoginPasswordCredentialsRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<LoginUserPasswordRequest, LoginUserPasswordResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Tokens)
            : CreateFailure(response.Error);
    }

    private static LoginUserPasswordRequest Map(LoginPasswordCredentialsRequest request)
    {
        return new LoginUserPasswordRequest(request.Login, request.Password);
    }

    private static IActionResult CreateSuccess(UserTokens tokens)
    {
        var response = new LoginPasswordCredentialsResponse(tokens);
        return new OkObjectResult(response);
    }

    private static IActionResult CreateFailure(Error<LoginUserPasswordErrorCode> error)
    {
        return error.Code switch
        {
            LoginUserPasswordErrorCode.UserNotFound => new ForbidResult(),
            LoginUserPasswordErrorCode.PasswordCredentialsNotAllowed => new ForbidResult(),
            LoginUserPasswordErrorCode.InvalidCredentials => new ForbidResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}