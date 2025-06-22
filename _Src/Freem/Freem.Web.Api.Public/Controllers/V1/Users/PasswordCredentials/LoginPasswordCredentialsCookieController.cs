using System.ComponentModel.DataAnnotations;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Password.Login;
using Freem.Entities.Users;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.DTO.Users.LoginPassword;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Users.PasswordCredentials;

[Tags(ControllerTags.User, ControllerTags.PasswordCredentials)]
[Route("api/v1/user/password-credentials/login/cookie")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class LoginPasswordCredentialsCookieController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public LoginPasswordCredentialsCookieController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPost]
    [EndpointSummary("Login using password credentials with cookie response")]
    public async Task<IActionResult> LoginAsync(
        [Required] [FromBody] LoginPasswordCredentialsRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<LoginUserPasswordRequest, LoginUserPasswordResponse>(context, request, cancellationToken);
        
        return response.Success
            ? CreateSuccess(Response, response.Tokens)
            : CreateFailure(response.Error);
    }

    private static LoginUserPasswordRequest Map(LoginPasswordCredentialsRequest request)
    {
        return new LoginUserPasswordRequest(request.Login, request.Password);
    }

    private static IActionResult CreateSuccess(HttpResponse response, UserTokens tokens)
    {
        response.Cookies.Append(CookieNames.AccessToken, tokens.AccessToken, new CookieOptions
        {
            //HttpOnly = true,
            //Secure = true,
            Expires = DateTimeOffset.MaxValue
        });
        
        response.Cookies.Append(CookieNames.RefreshToken, tokens.RefreshToken, new CookieOptions
        {
            //HttpOnly = true,
            //Secure = true,
            Path = "/api/v1/user/cookie-tokens/refresh",
            Expires = DateTimeOffset.MaxValue
        });
        
        return new OkResult();
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