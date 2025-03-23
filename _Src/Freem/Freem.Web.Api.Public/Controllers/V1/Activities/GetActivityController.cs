using System.ComponentModel.DataAnnotations;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Activities.Get;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts.Activities;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Authorize]
[Route("api/v1/activities/{activityId}")]
public sealed class GetActivityController : BaseController
{
    private readonly UseCaseContextProvider _provider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public GetActivityController(
        UseCaseContextProvider provider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _provider = provider;
        _executor = executor;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActivityResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] string activityId,
        CancellationToken cancellationToken = default)
    {
        var context = _provider.Get();
        var request = Map(activityId);

        var response = await _executor.ExecuteAsync<GetActivityRequest, GetActivityResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Activity)
            : CreateFailure(response.Error);
    }

    private static GetActivityRequest Map(string activityIdString)
    {
        var activityId = new ActivityIdentifier(activityIdString);
        return new GetActivityRequest(activityId);
    }

    private static IActionResult CreateSuccess(Activity activity)
    {
        var response = new ActivityResponse(activity.Id, activity.Name, activity.Status, activity.Tags);
        return new OkObjectResult(response);
    }
    
    private static IActionResult CreateFailure(Error<GetActivityErrorCode> error)
    {
        return error.Code switch
        {
            GetActivityErrorCode.ActivityNotFound => new NotFoundResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}