using System.ComponentModel.DataAnnotations;
using Freem.Domain.Entities.Activities;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Activities.Find;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Route("api/v1/activities/by-name")]
[Tags(ControllerTags.Activities)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<ActivityResponse>))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class FindActivityByNameController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public FindActivityByNameController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpGet]
    [EndpointSummary("Find activities by name")]
    public async Task<IActionResult> FindAsync(
        [Required] [FromQuery] FindActivityByNameRequest query,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(query);
        
        var response = await _mediator.ExecuteAsync<FindActivityRequest, Result<FindActivityResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(Response, response.Value.Activities, response.Value.TotalCount)
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

    private static IActionResult CreateFailure(Error error)
    {
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}