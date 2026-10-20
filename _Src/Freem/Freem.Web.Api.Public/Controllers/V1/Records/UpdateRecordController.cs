using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Records;
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
using ApiUpdateRecordRequest = Freem.Web.Api.Public.Contracts.DTO.Records.UpdateRecordRequest;
using UseCaseUpdateRecordRequest = Freem.Entities.UseCases.Contracts.Records.Update.UpdateRecordRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Route("api/v1/records/{recordId:required}")]
[Tags(ControllerTags.Records)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class UpdateRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;
    
    public UpdateRecordController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpPut]
    [EndpointSummary("Update record by id")]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromRoute] string recordId,
        [Required] [FromBody] ApiUpdateRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(recordId, body);

        var response = await _mediator.ExecuteAsync<UseCaseUpdateRecordRequest, Result<Error>>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static UseCaseUpdateRecordRequest Map(string recordIdString, ApiUpdateRecordRequest request)
    {
        var recordId = RecordId.Parse(recordIdString, null);

        var activities = request.Activities is not null
            ? new UpdateField<RelatedActivityIdsCollection>(new RelatedActivityIdsCollection(request.Activities.Value))
            : null;
        
        var tags = request.Tags?.Value != null
            ? new UpdateField<RelatedTagIdsCollection>(new RelatedTagIdsCollection(request.Tags.Value))
            : null;
        
        return new UseCaseUpdateRecordRequest(recordId)
        {
            Period = request.Period?.Map(),
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
        if (error.Type == DomainErrorCatalog.Types.RelatedEntitiesNotFound)
            return new UnprocessableEntityResult();
        if (error.Type == DomainErrorCatalog.Types.NothingToDo)
            return new BadRequestResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}