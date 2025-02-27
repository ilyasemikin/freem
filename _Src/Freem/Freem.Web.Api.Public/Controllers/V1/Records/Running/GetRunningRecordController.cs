using Freem.Entities.RunningRecords;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.RunningRecords.Get;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts.Records.Running;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Records.Running;

[Authorize]
[Route("api/v1/records/running")]
public sealed class GetRunningRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public GetRunningRecordController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetRunningRecordRequest))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = GetRunningRecordRequest.Instance;

        var response = await _executor.ExecuteAsync<GetRunningRecordRequest, GetRunningRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Record)
            : CreateFailure(response.Error);
    }

    private static IActionResult CreateSuccess(RunningRecord record)
    {
        var response = new RunningRecordResponse(record.Id, record.Activities, record.Tags)
        {
            Name = record.Name,
            Description = record.Description
        };

        return new OkObjectResult(response);
    }

    private static IActionResult CreateFailure(Error<GetRunningRecordErrorCode> error)
    {
        return error.Code switch
        {
            GetRunningRecordErrorCode.RunningRecordNotFound => new NotFoundResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}