using Freem.Domain;
using Freem.Domain.Entities.RunningRecords;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.RunningRecords.Get;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Records.Running;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1.Records.Running;

[Route("api/v1/records/running")]
[Tags(ControllerTags.RunningRecords)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetRunningRecordRequest))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class GetRunningRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public GetRunningRecordController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpGet]
    [EndpointSummary("Get running record")]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = GetRunningRecordRequest.Instance;

        var response = await _mediator.ExecuteAsync<GetRunningRecordRequest, Result<GetRunningRecordResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Value.Record)
            : CreateFailure(response.Error);
    }

    private static IActionResult CreateSuccess(RunningRecord record)
    {
        var response = new RunningRecordResponse(record.Id, record.StartAt, record.Activities, record.Tags)
        {
            Name = record.Name,
            Description = record.Description
        };

        return new OkObjectResult(response);
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new NotFoundResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}