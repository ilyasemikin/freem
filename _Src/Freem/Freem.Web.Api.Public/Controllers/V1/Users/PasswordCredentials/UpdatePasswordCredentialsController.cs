using System.ComponentModel.DataAnnotations;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Password.Update;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts.Users.LoginPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Users.PasswordCredentials;

[Authorize]
[Route("api/v1/user/password-credentials")]
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
    public async Task<ActionResult> UpdateAsync(
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

    private static ActionResult CreateFailure(Error<UpdateLoginCredentialsErrorCode> error)
    {
        throw new NotImplementedException();
    }
}