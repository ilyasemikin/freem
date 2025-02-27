using System.ComponentModel.DataAnnotations;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.RunningRecords.Stop;
using Freem.Time.Abstractions;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiStopRunningRecordRequest = Freem.Web.Api.Public.Contracts.Records.Running.StopRunningRecordRequest;
using UseCaseStopRunningRecordRequest = Freem.Entities.UseCases.Contracts.RunningRecords.Stop.StopRunningRecordRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Records.Running;

[Authorize]
[Route("api/v1/records/running/stop")]
public sealed class StopRunningRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;
    private readonly ICurrentTimeGetter _currentTimeGetter;

    public StopRunningRecordController(
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> StopAsync(
        [Required] [FromBody] ApiStopRunningRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<UseCaseStopRunningRecordRequest, StopRunningRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private UseCaseStopRunningRecordRequest Map(ApiStopRunningRecordRequest request)
    {
        var endAt = request.EndAt ?? _currentTimeGetter.Get();
        return new UseCaseStopRunningRecordRequest(endAt);
    }

    private static IActionResult CreateFailure(Error<StopRunningRecordErrorCode> error)
    {
        return error.Code switch
        {
            StopRunningRecordErrorCode.NothingToStop => new NotFoundResult(),
            StopRunningRecordErrorCode.EndAtToEarly => new UnprocessableEntityResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}