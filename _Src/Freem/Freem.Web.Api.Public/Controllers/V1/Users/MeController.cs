using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Get;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Entities.Users;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Autherization;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Users;
using Freem.Web.Api.Public.Contracts.DTO.Users.Settings;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Users;

[Authorize(JwtAuthorizationPolicy.Name)]
[Route("api/v1/user/me")]
[Tags(ControllerTags.User, ControllerTags.Settings)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MeResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class MeController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public MeController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    [EndpointSummary("Get information about me")]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = GetUserRequest.Instance;

        var response = await _executor.ExecuteAsync<GetUserRequest, GetUserResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.User)
            : CreateFailure(response.Error);
    }
    
    private static IActionResult CreateSuccess(User user)
    {
        var response = new MeResponse(user.Id, user.Nickname);
        return new OkObjectResult(response);
    }

    private static IActionResult CreateFailure(Error<GetUserErrorCode> error)
    {
        return error.Code switch
        {
            GetUserErrorCode.UserNotFound => new UnauthorizedResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}