using System.ComponentModel.DataAnnotations;
using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.RunningRecords.Update;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiUpdateRunningRecordRequest = Freem.Web.Api.Public.Contracts.Records.Running.UpdateRunningRecordRequest;
using UseCaseUpdateRunningRecordRequest = Freem.Entities.UseCases.Contracts.RunningRecords.Update.UpdateRunningRecordRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Records.Running;

[Authorize]
[Route("api/v1/records/running")]
public sealed class UpdateRunningRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public UpdateRunningRecordController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync(
        [Required] [FromBody] ApiUpdateRunningRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<UseCaseUpdateRunningRecordRequest, UpdateRunningRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static UseCaseUpdateRunningRecordRequest Map(ApiUpdateRunningRecordRequest request)
    {
        var activities = request.Activities is not null
            ? new UpdateField<RelatedActivitiesCollection>(new RelatedActivitiesCollection(request.Activities.Value))
            : null;
        
        var tags = request.Tags is not null
            ? new UpdateField<RelatedTagsCollection>(new RelatedTagsCollection(request.Tags.Value))
            : null;
        
        return new UseCaseUpdateRunningRecordRequest()
        {
            Name = request.Name?.Map(),
            Description = request.Description?.Map(),
            Activities = activities,
            Tags = tags
        };
    }

    private static ActionResult CreateFailure(Error<UpdateRunningRecordErrorCode> error)
    {
        throw new NotImplementedException();
    }
}