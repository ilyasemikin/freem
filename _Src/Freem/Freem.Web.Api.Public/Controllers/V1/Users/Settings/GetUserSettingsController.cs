using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
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
    public async Task<ActionResult<UserSettingsResponse>> GetAsync(CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = GetUserSettingsRequest.Instance;

        var response = await _executor.ExecuteAsync<GetUserSettingsRequest, GetUserSettingsResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Settings)
            : CreateFailure(response.Error);
    }

    private static UserSettingsResponse CreateSuccess(UserSettings settings)
    {
        return new UserSettingsResponse(settings.DayUtcOffset);
    }

    private static ActionResult<UserSettingsResponse> CreateFailure(Error<GetUserSettingsErrorCode> error)
    {
        throw new NotImplementedException();
    }
}