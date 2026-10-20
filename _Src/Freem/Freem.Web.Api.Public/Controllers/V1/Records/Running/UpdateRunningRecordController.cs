using System.ComponentModel.DataAnnotations;
using Freem.Domain;
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
using ApiUpdateRunningRecordRequest = Freem.Web.Api.Public.Contracts.DTO.Records.Running.UpdateRunningRecordRequest;
using UseCaseUpdateRunningRecordRequest = Freem.Entities.UseCases.Contracts.RunningRecords.Update.UpdateRunningRecordRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Records.Running;

[Route("api/v1/records/running")]
[Tags(ControllerTags.RunningRecords)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class UpdateRunningRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public UpdateRunningRecordController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpPut]
    [EndpointSummary("Update running record")]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromBody] ApiUpdateRunningRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(body);

        var response = await _mediator.ExecuteAsync<UseCaseUpdateRunningRecordRequest, Result<Error>>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static UseCaseUpdateRunningRecordRequest Map(ApiUpdateRunningRecordRequest request)
    {
        var activities = request.Activities is not null
            ? new UpdateField<RelatedActivityIdsCollection>(new RelatedActivityIdsCollection(request.Activities.Value))
            : null;
        
        var tags = request.Tags is not null
            ? new UpdateField<RelatedTagIdsCollection>(new RelatedTagIdsCollection(request.Tags.Value))
            : null;
        
        return new UseCaseUpdateRunningRecordRequest()
        {
            Name = request.Name?.Map(),
            Description = request.Description?.Map(),
            Activities = activities,
            Tags = tags
        };
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new NotFoundResult();
        if (error.Type == DomainErrorCatalog.Types.NothingToDo)
            return new BadRequestResult();
        if (error.Type == DomainErrorCatalog.Types.RelatedEntitiesNotFound)
            return new UnprocessableEntityResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}