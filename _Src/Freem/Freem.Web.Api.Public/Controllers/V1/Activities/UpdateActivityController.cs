using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Activities;
using Freem.Domain.Relations.Collections;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Mappers;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using ApiUpdateActivityRequest = Freem.Web.Api.Public.Contracts.DTO.Activities.UpdateActivityRequest;
using UseCaseUpdateActivityRequest = Freem.Entities.UseCases.Contracts.Activities.Update.UpdateActivityRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Route("api/v1/activities/{activityId:required}")]
[Tags(ControllerTags.Activities)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class UpdateActivityController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public UpdateActivityController(
        UseCaseContextProvider contextProvider,
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);

        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpPut]
    [EndpointSummary("Update activity by id")]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromRoute] string activityId,
        [Required] [FromBody] ApiUpdateActivityRequest body,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(activityId, body);

        var response =
            await _mediator.ExecuteAsync<UseCaseUpdateActivityRequest, Result<Error>>(context, request,
                cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static UseCaseUpdateActivityRequest Map(string activityIdString, ApiUpdateActivityRequest request)
    {
        var activityId = ActivityId.Parse(activityIdString, null);

        var tags = request.Tags is not null
            ? new UpdateField<RelatedTagIdsCollection>(new RelatedTagIdsCollection(request.Tags.Value))
            : null;

        return new UseCaseUpdateActivityRequest(activityId)
        {
            Name = request.Name?.Map(),
            Tags = tags
        };
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new NotFoundResult();
        if (error.Type == DomainErrorCatalog.Types.RelatedEntitiesNotFound)
            return new UnprocessableEntityResult();
        if (error.Type == DomainErrorCatalog.Types.NothingToDo)
            return new BadRequestResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}