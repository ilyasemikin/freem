using System.ComponentModel.DataAnnotations;
using Freem.Entities.Records;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Entities.UseCases.Contracts.Records.PeriodList;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Authorize]
[Route("api/v1/records/by-period")]
public sealed class ListByPeriodRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;
    
    public ListByPeriodRecordController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<RecordResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] ListByPeriodRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(query);

        var response = await _executor.ExecuteAsync<PeriodListRequest, PeriodListResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(Response, response.Records, response.TotalCount)
            : CreateFailure(response.Error);
    }

    private static PeriodListRequest Map(ListByPeriodRequest request)
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
                yield return new RecordResponse(record.Id, record.Activities, record.Tags)
                {
                    Name = record.Name,
                    Description = record.Description
                };
        }
    }

    private static IActionResult CreateFailure(Error<PeriodListErrorCode> error)
    {
        return error.Code switch
        {
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}