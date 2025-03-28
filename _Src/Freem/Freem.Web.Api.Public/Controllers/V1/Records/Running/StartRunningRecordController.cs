﻿using System.ComponentModel.DataAnnotations;
using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
using Freem.Time.Abstractions;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiStartRunningRecordRequest = Freem.Web.Api.Public.Contracts.Records.Running.StartRunningRecordRequest;
using UseCaseStartRunningRecordRequest = Freem.Entities.UseCases.Contracts.RunningRecords.Start.StartRunningRecordRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Records.Running;

[Authorize]
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
    private readonly ICurrentTimeGetter _currentTimeGetter;

    public StartRunningRecordController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor, 
        ICurrentTimeGetter currentTimeGetter)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        ArgumentNullException.ThrowIfNull(currentTimeGetter);
        
        _contextProvider = contextProvider;
        _executor = executor;
        _currentTimeGetter = currentTimeGetter;
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
        var startAt = request.StartAt ?? _currentTimeGetter.Get();
        return new UseCaseStartRunningRecordRequest(startAt, new RelatedActivitiesCollection(request.Activities))
        {
            Name = request.Name,
            Description = request.Description,
            Tags = new RelatedTagsCollection(request.Tags)
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