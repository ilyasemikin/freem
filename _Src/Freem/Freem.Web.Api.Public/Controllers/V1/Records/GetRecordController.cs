using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Records.Get;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Records;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Route("api/v1/records/{recordId:required}")]
[Tags(ControllerTags.Records)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class GetRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;
    
    public GetRecordController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpGet]
    [EndpointSummary("Get record by id")]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] string recordId,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(recordId);

        var response = await _mediator.ExecuteAsync<GetRecordRequest, Result<GetRecordResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Value.Record)
            : CreateFailure(response.Error);
    }

    private static GetRecordRequest Map(string recordIdString)
    {
        var recordId = RecordId.Parse(recordIdString, null);
        return new GetRecordRequest(recordId);
    }

    private static IActionResult CreateSuccess(Record record)
    {
        var response = new RecordResponse(record.Id, record.Period, record.Activities, record.Tags)
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