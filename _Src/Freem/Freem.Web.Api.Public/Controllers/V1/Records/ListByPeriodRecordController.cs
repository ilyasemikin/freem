using System.ComponentModel.DataAnnotations;
using Freem.Domain.Entities.Records;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Entities.UseCases.Contracts.Records.PeriodList;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.DTO.Records;
using Freem.Web.Api.Public.OpenApi.Headers;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Route("api/v1/records/by-period")]
[Tags(ControllerTags.Records)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<RecordResponse>))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesHeader(HeaderNames.ItemsCount, StatusCode = StatusCodes.Status200OK)]
[ProducesHeader(HeaderNames.TotalItemsCount, StatusCode = StatusCodes.Status200OK)]
public sealed class ListByPeriodRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;
    
    public ListByPeriodRecordController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpGet]
    [EndpointSummary("List records by period")]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] ListRecordByPeriodRequest query,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(query);

        var response = await _mediator.ExecuteAsync<PeriodListRequest, Result<PeriodListResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(Response, response.Value.Records, response.Value.TotalCount)
            : CreateFailure(response.Error);
    }

    private static PeriodListRequest Map(ListRecordByPeriodRequest request)
    {
        var limit = new Limit(request.Limit);
        return new PeriodListRequest(request.Period, limit);
    }

    private static IActionResult CreateSuccess(
        HttpResponse response, IReadOnlyList<Record> records, int totalCount)
    {
        response.Headers.Append(HeaderNames.ItemsCount, records.Count.ToString());
        response.Headers.Append(HeaderNames.TotalItemsCount, totalCount.ToString());

        var value = MapRecords(records);
        return new OkObjectResult(value);

        static async IAsyncEnumerable<RecordResponse> MapRecords(IReadOnlyList<Record> records)
        {
            foreach (var record in records)
                yield return new RecordResponse(record.Id, record.Period, record.Activities, record.Tags)
                {
                    Name = record.Name,
                    Description = record.Description
                };
        }
    }

    private static IActionResult CreateFailure(Error error)
    {
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}