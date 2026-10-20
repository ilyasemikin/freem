using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Entities.Records;
using Freem.Domain.Relations.Collections;
using Freem.Entities.UseCases.Context;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using ApiCreateRecordRequest = Freem.Web.Api.Public.Contracts.DTO.Records.CreateRecordRequest;
using ApiCreateRecordResponse = Freem.Web.Api.Public.Contracts.DTO.Records.CreateRecordResponse;
using UseCaseCreateRecordRequest = Freem.Entities.UseCases.Contracts.Records.Create.CreateRecordRequest;
using UseCaseCreateRecordResponse = Freem.Entities.UseCases.Contracts.Records.Create.CreateRecordResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Route("api/v1/records")]
[Tags(ControllerTags.Records)]
[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreateRecordResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class CreateRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public CreateRecordController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpPost]
    [EndpointSummary("Create record")]
    public async Task<IActionResult> CreateAsync(
        [Required] [FromBody] ApiCreateRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(body);

        var response = await _mediator.ExecuteAsync<UseCaseCreateRecordRequest, Result<UseCaseCreateRecordResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Value.Record)
            : CreateFailure(response.Error);
    }

    private static UseCaseCreateRecordRequest Map(ApiCreateRecordRequest request)
    {
        return new UseCaseCreateRecordRequest(request.Period, new RelatedActivityIdsCollection(request.Activities))
        {
            Name = request.Name,
            Description = request.Description,
            Tags = new RelatedTagIdsCollection(request.Tags)
        };
    }

    private static IActionResult CreateSuccess(Record record)
    {
        var response = new ApiCreateRecordResponse(record.Id);
        return new CreatedResult()
        {
            Value = response
        };
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.RelatedEntitiesNotFound)
            return new UnprocessableEntityResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}