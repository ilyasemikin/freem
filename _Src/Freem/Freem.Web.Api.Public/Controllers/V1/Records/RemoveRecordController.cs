using System.ComponentModel.DataAnnotations;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Records.Remove;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Autherization;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Authorize(JwtAuthorizationPolicy.Name)]
[Route("api/v1/records/{recordId:required}")]
[Tags(ControllerTags.Records)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class RemoveRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;
    
    public RemoveRecordController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }
    
    [HttpDelete]
    [EndpointSummary("Remove record by id")]
    public async Task<IActionResult> RemoveAsync(
        [Required] [FromRoute] string recordId,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(recordId);

        var response = await _executor.ExecuteAsync<RemoveRecordRequest, RemoveRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static RemoveRecordRequest Map(string recordIdString)
    {
        var recordId = new RecordIdentifier(recordIdString);
        return new RemoveRecordRequest(recordId);
    }

    private static IActionResult CreateFailure(Error<RemoveRecordErrorCode> error)
    {
        return error.Code switch
        {
            RemoveRecordErrorCode.RecordNotFound => new NotFoundResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}