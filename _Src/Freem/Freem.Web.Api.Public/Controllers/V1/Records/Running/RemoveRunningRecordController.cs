using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.RunningRecords.Remove;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Autherization;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Records.Running;

[Authorize(JwtAuthorizationPolicy.Name)]
[Route("api/v1/records/running")]
[Tags(ControllerTags.RunningRecords)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    [EndpointSummary("Remove running record")]
    public async Task<IActionResult> RemoveAsync(CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = RemoveRunningRecordRequest.Instance;

        var response = await _executor.ExecuteAsync<RemoveRunningRecordRequest, RemoveRunningRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static IActionResult CreateFailure(Error<RemoveRunningRecordErrorCode> error)
    {
        return error.Code switch
        {
            RemoveRunningRecordErrorCode.RunningRecordNotFound => new NotFoundResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}