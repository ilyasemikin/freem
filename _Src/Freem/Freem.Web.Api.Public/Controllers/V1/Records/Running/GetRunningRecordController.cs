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
    public async Task<ActionResult<RunningRecordResponse>> GetAsync(CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = GetRunningRecordRequest.Instance;

        var response = await _executor.ExecuteAsync<GetRunningRecordRequest, GetRunningRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Record)
            : CreateFailure(response.Error);
    }

    private static RunningRecordResponse CreateSuccess(RunningRecord record)
    {
        return new RunningRecordResponse(record.Id, record.Activities, record.Tags)
        {
            Name = record.Name,
            Description = record.Description
        };
    }

    private static ActionResult<RunningRecordResponse> CreateFailure(Error<GetRunningRecordErrorCode> error)
    {
        throw new NotImplementedException();
    }
}