using System.ComponentModel.DataAnnotations;
using Freem.Entities.Activities;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Activities.List;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.Activities;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiListActivityRequest = Freem.Web.Api.Public.Contracts.Activities.ListActivityRequest;
using UseCaseListActivityRequest = Freem.Entities.UseCases.Contracts.Activities.List.ListActivityRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Authorize]
[Route("api/v1/activities")]
public sealed class ListActivityController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public ListActivityController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<ActivityResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] ApiListActivityRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(query);

        var response = await _executor.ExecuteAsync<UseCaseListActivityRequest, ListActivityResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(Response, response.Activities, response.TotalCount)
            : CreateFailure(response.Error);
    }

    private static UseCaseListActivityRequest Map(ApiListActivityRequest request)
    {
        var limit = new Limit(request.Limit);
        var offset = new Offset(request.Offset);
        
        return new UseCaseListActivityRequest(limit, offset);
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

    private static IActionResult CreateFailure(Error<ListActivityErrorCode> error)
    {
        return error.Code switch
        {
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}