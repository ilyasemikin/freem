using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Activities;
using Freem.Domain.Entities.Activities;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Activities.Get;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Route("api/v1/activities/{activityId:required}")]
[Tags(ControllerTags.Activities)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActivityResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class GetActivityController : BaseController
{
    private readonly UseCaseContextProvider _provider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public GetActivityController(
        UseCaseContextProvider provider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _provider = provider;
        _mediator = mediator;
    }

    [HttpGet]
    [EndpointSummary("Get activity by id")]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] string activityId,
        CancellationToken cancellationToken = default)
    {
        if (!_provider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(activityId);

        var response = await _mediator.ExecuteAsync<GetActivityRequest, Result<GetActivityResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Value.Activity)
            : CreateFailure(response.Error);
    }

    private static GetActivityRequest Map(string activityIdString)
    {
        var activityId = ActivityId.Parse(activityIdString, null);
        return new GetActivityRequest(activityId);
    }

    private static IActionResult CreateSuccess(Activity activity)
    {
        var response = new ActivityResponse(activity.Id, activity.Name, activity.Status, activity.Tags);
        return new OkObjectResult(response);
    }
    
    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new NotFoundResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}