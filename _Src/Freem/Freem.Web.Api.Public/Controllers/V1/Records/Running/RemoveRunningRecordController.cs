using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.RunningRecords.Remove;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Records.Running;

[Authorize]
[Route("api/v1/records/running")]
public sealed class RemoveRunningRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public RemoveRunningRecordController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveAsync(CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = RemoveRunningRecordRequest.Instance;

        var response = await _executor.ExecuteAsync<RemoveRunningRecordRequest, RemoveRunningRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static ActionResult CreateFailure(Error<RemoveRunningRecordErrorCode> error)
    {
        throw new NotImplementedException();
    }
}