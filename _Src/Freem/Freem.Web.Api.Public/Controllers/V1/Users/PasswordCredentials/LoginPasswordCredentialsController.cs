using System.ComponentModel.DataAnnotations;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Password.Login;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts.Users.LoginPassword;
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
    public async Task<ActionResult<LoginPasswordCredentialsResponse>> LoginAsync(
        [Required] [FromBody] LoginPasswordCredentialsRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<LoginUserPasswordRequest, LoginUserPasswordResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.AccessToken, response.RefreshToken)
            : CreateFailure(response.Error);
    }

    private static LoginUserPasswordRequest Map(LoginPasswordCredentialsRequest request)
    {
        return new LoginUserPasswordRequest(request.Login, request.Password);
    }

    private static LoginPasswordCredentialsResponse CreateSuccess(string accessToken, string refreshToken)
    {
        return new LoginPasswordCredentialsResponse(accessToken, refreshToken);
    }

    private static ActionResult<LoginPasswordCredentialsResponse> CreateFailure(Error<LoginUserPasswordErrorCode> error)
    {
        throw new NotImplementedException();
    }
}