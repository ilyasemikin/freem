using System.ComponentModel.DataAnnotations;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Settings.Update;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiUpdateUserSettingsRequest = Freem.Web.Api.Public.Contracts.Users.Settings.UpdateUserSettingsRequest;
using UseCaseUpdateUserSettingsRequest = Freem.Entities.UseCases.Contracts.Users.Settings.Update.UpdateUserSettingsRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Users.Settings;

[Authorize]
[Route("api/v1/user/settings")]
public sealed class UpdateSettingsController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public UpdateSettingsController(
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
        [Required] [FromBody] ApiUpdateUserSettingsRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<UseCaseUpdateUserSettingsRequest, UpdateUserSettingsResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static UseCaseUpdateUserSettingsRequest Map(ApiUpdateUserSettingsRequest request)
    {
        return new UseCaseUpdateUserSettingsRequest()
        {
            UtcOffset = request.DayUtcOffset?.Map()
        };
    }

    private static ActionResult CreateFailure(Error<UpdateUserSettingsErrorCode> error)
    {
        throw new NotImplementedException();
    }
}