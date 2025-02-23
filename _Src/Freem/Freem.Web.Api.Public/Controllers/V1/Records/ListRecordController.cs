using System.ComponentModel.DataAnnotations;
using Freem.Entities.Records;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Entities.UseCases.Contracts.Records.List;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiListRecordRequest = Freem.Web.Api.Public.Contracts.Records.ListRecordRequest;
using UseCaseListRecordRequest = Freem.Entities.UseCases.Contracts.Records.List.ListRecordRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Authorize]
[Route("api/v1/records")]
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
    public async Task<ActionResult<IAsyncEnumerable<RecordResponse>>> ListAsync(
        [Required] [FromRoute] ApiListRecordRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(query);

        var response = await _executor.ExecuteAsync<UseCaseListRecordRequest, ListRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok(CreateSuccess(Response, response.Records, response.TotalCount))
            : CreateFailure(response.Error);
    }

    private static UseCaseListRecordRequest Map(ApiListRecordRequest request)
    {
        var limit = new Limit(request.Limit);
        var offset = new Offset(request.Offset);
        return new UseCaseListRecordRequest(limit, offset);
    }

    private static async IAsyncEnumerable<RecordResponse> CreateSuccess(
        HttpResponse response, IReadOnlyList<Record> records, int totalCount)
    {
        response.Headers.Append(HeaderNames.ItemsCount, records.Count.ToString());
        response.Headers.Append(HeaderNames.TotalItemsCount, totalCount.ToString());

        foreach (var record in records)
            yield return new RecordResponse(record.Id, record.Activities, record.Tags)
            {
                Name = record.Name,
                Description = record.Description
            };

        await Task.CompletedTask;
    }

    private static ActionResult<IAsyncEnumerable<RecordResponse>> CreateFailure(Error<ListRecordErrorCode> error)
    {
        throw new NotImplementedException();
    }
}