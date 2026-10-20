using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Relations.Collections;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Clock.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
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
    private readonly IRequestMediator<HandlerContext> _mediator;
    private readonly IClock _clock;

    public StartRunningRecordController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator, 
        IClock clock)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(clock);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
        _clock = clock;
    }

    [HttpPost]
    [EndpointSummary("Start running record")]
    public async Task<IActionResult> StartAsync(
        [Required] [FromBody] ApiStartRunningRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(body);

        var response = await _mediator.ExecuteAsync<UseCaseStartRunningRecordRequest, Result<StartRunningRecordResponse, Error>>(context, request, cancellationToken);

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

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.RelatedEntitiesNotFound)
            return new UnprocessableEntityResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}