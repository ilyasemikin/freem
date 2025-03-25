using System.ComponentModel.DataAnnotations;
using Freem.Entities.Records;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Entities.UseCases.Contracts.Records.List;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.Records;
using Freem.Web.Api.Public.OpenApi.Headers;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiListRecordRequest = Freem.Web.Api.Public.Contracts.Records.ListRecordRequest;
using UseCaseListRecordRequest = Freem.Entities.UseCases.Contracts.Records.List.ListRecordRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Authorize]
[Route("api/v1/records")]
[Tags(ControllerTags.Records)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<RecordResponse>))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesHeader(HeaderNames.ItemsCount, StatusCode = StatusCodes.Status200OK)]
[ProducesHeader(HeaderNames.TotalItemsCount, StatusCode = StatusCodes.Status200OK)]
public class ListRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;
    
    public ListRecordController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    [EndpointSummary("List all records")]
    public async Task<IActionResult> ListAsync(
        [Required] [FromRoute] ApiListRecordRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(query);

        var response = await _executor.ExecuteAsync<UseCaseListRecordRequest, ListRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(Response, response.Records, response.TotalCount)
            : CreateFailure(response.Error);
    }

    private static UseCaseListRecordRequest Map(ApiListRecordRequest request)
    {
        var limit = new Limit(request.Limit);
        var offset = new Offset(request.Offset);
        return new UseCaseListRecordRequest(limit, offset);
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

    private static IActionResult CreateFailure(Error<ListRecordErrorCode> error)
    {
        return error.Code switch
        {
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}