using System.ComponentModel.DataAnnotations;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Register;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using ApiRegisterUserResponse = Freem.Web.Api.Public.Contracts.DTO.Users.RegisterUserResponse;
using ApiRegisterUserRequest = Freem.Web.Api.Public.Contracts.DTO.Users.RegisterUserRequest;
using RegisterUserRequest = Freem.Entities.UseCases.Contracts.Users.Register.RegisterUserRequest;
using RegisterUserResponse = Freem.Entities.UseCases.Contracts.Users.Register.RegisterUserResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Users;

[Route("api/v1/user/register")]
[Tags(ControllerTags.User)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiRegisterUserResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class RegisterUserController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public RegisterUserController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPost]
    [EndpointSummary("Register user")]
    public async Task<IActionResult> RegisterAsync(
        [Required] [FromBody] ApiRegisterUserRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = new RegisterUserRequest(body.Nickname);

        var response = await _executor.ExecuteAsync<RegisterUserRequest, RegisterUserResponse>(context, request, cancellationToken);

        if (!response.Success)
            return CreateFailure(response.Error);

        var result = new ApiRegisterUserResponse(response.UserId);
        return Ok(result);
    }

    private static IActionResult CreateFailure(Error<RegisterUserErrorCode> error)
    {
        return error.Code switch
        {
            RegisterUserErrorCode.UserAlreadyRegistered => CreateResult(StatusCodes.Status409Conflict),
            _ => CreateResult(StatusCodes.Status500InternalServerError)
        };
    }
}
