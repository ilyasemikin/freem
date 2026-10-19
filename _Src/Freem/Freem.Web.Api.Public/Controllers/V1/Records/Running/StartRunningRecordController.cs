using System.ComponentModel.DataAnnotations;
using Freem.Domain.Relations.Collections;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Clock.Abstractions;
using ApiStartRunningRecordRequest = Freem.Web.Api.Public.Contracts.DTO.Records.Running.StartRunningRecordRequest;
using UseCaseStartRunningRecordRequest = Freem.Entities.UseCases.Contracts.RunningRecords.Start.StartRunningRecordRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Records.Running;

[Route("api/v1/records/running/start")]
[Tags(ControllerTags.RunningRecords)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class StartRunningRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;
    private readonly IClock _clock;

    public StartRunningRecordController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor, 
        IClock clock)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        ArgumentNullException.ThrowIfNull(clock);
        
        _contextProvider = contextProvider;
        _executor = executor;
        _clock = clock;
    }

    [HttpPost]
    [EndpointSummary("Start running record")]
    public async Task<IActionResult> StartAsync(
        [Required] [FromBody] ApiStartRunningRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<UseCaseStartRunningRecordRequest, StartRunningRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private UseCaseStartRunningRecordRequest Map(ApiStartRunningRecordRequest request)
    {
        var startAt = request.StartAt ?? _clock.Get();
        return new UseCaseStartRunningRecordRequest(startAt, new RelatedActivityIdsCollection(request.Activities))
        {
            Name = request.Name,
            Description = request.Description,
            Tags = new RelatedTagIdsCollection(request.Tags)
        };
    }

    private static IActionResult CreateFailure(Error<StartRunningRecordErrorCode> error)
    {
        return error.Code switch
        {
            StartRunningRecordErrorCode.RelatedActivitiesNotFound => new UnprocessableEntityResult(),
            StartRunningRecordErrorCode.RelatedTagsNotFound => new UnprocessableEntityResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}