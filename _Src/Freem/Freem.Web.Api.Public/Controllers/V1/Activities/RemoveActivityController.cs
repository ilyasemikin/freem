using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Activities;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Activities.Remove;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Route("api/v1/activities/{activityId:required}")]
[Tags(ControllerTags.Activities)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class RemoveActivityController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public RemoveActivityController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpDelete]
    [EndpointSummary("Remove activity by id")]
    public async Task<IActionResult> RemoveAsync(
        [Required] [FromRoute] string activityId,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(activityId);

        var response = await _mediator.ExecuteAsync<RemoveActivityRequest, Result<Error>>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static RemoveActivityRequest Map(string activityIdString)
    {
        var activityId = ActivityId.Parse(activityIdString, null);
        return new RemoveActivityRequest(activityId);
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new NotFoundResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}