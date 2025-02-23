using System.ComponentModel.DataAnnotations;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Password.Register;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts.Users.LoginPassword;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Users.PasswordCredentials;

[Route("api/v1/user/password-credentials/register")]
public sealed class RegisterPasswordCredentialsController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public RegisterPasswordCredentialsController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPost]
    public async Task<ActionResult> RegisterAsync(
        [Required] [FromBody] RegisterPasswordCredentialsRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static RegisterUserPasswordRequest Map(RegisterPasswordCredentialsRequest request)
    {
        return new RegisterUserPasswordRequest(request.Nickname, request.Login, request.Password);
    }

    private static ActionResult CreateFailure(Error<RegisterUserPasswordErrorCode> error)
    {
        throw new NotImplementedException();
    }
}