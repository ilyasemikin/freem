using System.ComponentModel.DataAnnotations;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.Records.Update;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Mappers;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiUpdateRecordRequest = Freem.Web.Api.Public.Contracts.Records.UpdateRecordRequest;
using UseCaseUpdateRecordRequest = Freem.Entities.UseCases.Contracts.Records.Update.UpdateRecordRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Authorize]
[Route("api/v1/records/{recordId}")]
public sealed class UpdateRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;
    
    public UpdateRecordController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromRoute] string recordId,
        [Required] [FromBody] ApiUpdateRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(recordId, body);

        var response = await _executor.ExecuteAsync<UseCaseUpdateRecordRequest, UpdateRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static UseCaseUpdateRecordRequest Map(string recordIdString, ApiUpdateRecordRequest request)
    {
        var recordId = new RecordIdentifier(recordIdString);

        var activities = request.Activities is not null
            ? new UpdateField<RelatedActivitiesCollection>(new RelatedActivitiesCollection(request.Activities.Value))
            : null;
        
        var tags = request.Tags is not null
            ? new UpdateField<RelatedTagsCollection>(new RelatedTagsCollection(request.Tags.Value))
            : null;
        
        return new UseCaseUpdateRecordRequest(recordId)
        {
            Name = request.Name?.Map(),
            Description = request.Description?.Map(),
            Activities = activities,
            Tags = tags
        };
    }

    private static IActionResult CreateFailure(Error<UpdateRecordErrorCode> error)
    {
        return error.Code switch
        {
            UpdateRecordErrorCode.RecordNotFound => new NotFoundResult(),
            UpdateRecordErrorCode.RelatedActivitiesNotFound => new UnprocessableEntityResult(),
            UpdateRecordErrorCode.RelatedTagsNotFound => new UnprocessableEntityResult(),
            UpdateRecordErrorCode.NothingToUpdate => new BadRequestResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}