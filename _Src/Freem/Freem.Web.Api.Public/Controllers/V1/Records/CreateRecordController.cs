using System.ComponentModel.DataAnnotations;
using Freem.Entities.Records;
using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Autherization;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiCreateRecordRequest = Freem.Web.Api.Public.Contracts.DTO.Records.CreateRecordRequest;
using ApiCreateRecordResponse = Freem.Web.Api.Public.Contracts.DTO.Records.CreateRecordResponse;
using UseCaseCreateRecordRequest = Freem.Entities.UseCases.Contracts.Records.Create.CreateRecordRequest;
using UseCaseCreateRecordResponse = Freem.Entities.UseCases.Contracts.Records.Create.CreateRecordResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Authorize(JwtAuthorizationPolicy.Name)]
[Route("api/v1/records")]
[Tags(ControllerTags.Records)]
[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreateRecordResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class CreateRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public CreateRecordController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPost]
    [EndpointSummary("Create record")]
    public async Task<IActionResult> CreateAsync(
        [Required] [FromBody] ApiCreateRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<UseCaseCreateRecordRequest, UseCaseCreateRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Record)
            : CreateFailure(response.Error);
    }

    private static UseCaseCreateRecordRequest Map(ApiCreateRecordRequest request)
    {
        return new UseCaseCreateRecordRequest(request.Period, new RelatedActivitiesCollection(request.Activities))
        {
            Name = request.Name,
            Description = request.Description,
            Tags = new RelatedTagsCollection(request.Tags)
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

    private static IActionResult CreateFailure(Error<CreateRecordErrorCode> error)
    {
        return error.Code switch
        {
            CreateRecordErrorCode.RelatedActivitiesNotFound => new UnprocessableEntityResult(),
            CreateRecordErrorCode.RelatedTagsNotFound => new UnprocessableEntityResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}