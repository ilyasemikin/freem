using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Entities.UseCases.Context;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Clock.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using ApiStopRunningRecordRequest = Freem.Web.Api.Public.Contracts.DTO.Records.Running.StopRunningRecordRequest;
using UseCaseStopRunningRecordRequest = Freem.Entities.UseCases.Contracts.RunningRecords.Stop.StopRunningRecordRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Records.Running;

[Route("api/v1/records/running/stop")]
[Tags(ControllerTags.RunningRecords)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class StopRunningRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;
    private readonly IClock _clock;

    public StopRunningRecordController(
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
    [EndpointSummary("Stop running record if exists")]
    public async Task<IActionResult> StopAsync(
        [Required] [FromBody] ApiStopRunningRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(body);

        var response = await _mediator.ExecuteAsync<UseCaseStopRunningRecordRequest, Result<Error>>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private UseCaseStopRunningRecordRequest Map(ApiStopRunningRecordRequest request)
    {
        var endAt = request.EndAt ?? _clock.Get();
        return new UseCaseStopRunningRecordRequest(endAt);
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new NotFoundResult();
        if (error.Type == DomainErrorCatalog.Types.PeriodIsInvalid)
            return new UnprocessableEntityResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}