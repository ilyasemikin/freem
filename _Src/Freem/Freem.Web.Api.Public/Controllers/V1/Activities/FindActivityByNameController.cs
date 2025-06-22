using System.ComponentModel.DataAnnotations;
using Freem.Entities.Activities;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Activities.Find;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Autherization;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Authorize(JwtAuthorizationPolicy.Name)]
[Route("api/v1/activities/by-name")]
[Tags(ControllerTags.Activities)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<ActivityResponse>))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class FindActivityByNameController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public FindActivityByNameController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    [EndpointSummary("Find activities by name")]
    public async Task<IActionResult> FindAsync(
        [Required] [FromQuery] FindActivityByNameRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(query);
        
        var response = await _executor.ExecuteAsync<FindActivityRequest, FindActivityResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(Response, response.Activities, response.TotalCount)
            : CreateFailure(response.Error);
    }

    private static FindActivityRequest Map(FindActivityByNameRequest request)
    {
        return new FindActivityRequest(request.SearchText);
    }
    
    private static IActionResult CreateSuccess(
        HttpResponse response, IReadOnlyList<Activity> activities, int totalCount)
    {
        response.Headers.Append(HeaderNames.ItemsCount, activities.Count.ToString());
        response.Headers.Append(HeaderNames.TotalItemsCount, totalCount.ToString());

        var value = MapActivities(activities);
        return new OkObjectResult(value);

        static async IAsyncEnumerable<ActivityResponse> MapActivities(IReadOnlyList<Activity> activities)
        {
            foreach (var activity in activities)
                yield return new ActivityResponse(activity.Id, activity.Name, activity.Status, activity.Tags);
        }
    }

    private static IActionResult CreateFailure(Error<FindActivityErrorCode> error)
    {
        return error.Code switch
        {
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}