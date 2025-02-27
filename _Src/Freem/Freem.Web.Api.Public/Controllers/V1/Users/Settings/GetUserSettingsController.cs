using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.Users;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts.Users.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Users.Settings;

[Authorize]
[Route("api/v1/user/settings")]
public sealed class GetUserSettingsController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public GetUserSettingsController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSettingsResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = GetUserSettingsRequest.Instance;

        var response = await _executor.ExecuteAsync<GetUserSettingsRequest, GetUserSettingsResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Settings)
            : CreateFailure(response.Error);
    }

    private static IActionResult CreateSuccess(UserSettings settings)
    {
        var response = new UserSettingsResponse(settings.DayUtcOffset);
        return new OkObjectResult(response);
    }

    private static IActionResult CreateFailure(Error<GetUserSettingsErrorCode> error)
    {
        return error.Code switch
        {
            GetUserSettingsErrorCode.UserNotFound => new UnauthorizedResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}