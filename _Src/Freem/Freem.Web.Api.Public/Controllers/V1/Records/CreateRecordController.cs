using System.ComponentModel.DataAnnotations;
using Freem.Entities.Records;
using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiCreateRecordRequest = Freem.Web.Api.Public.Contracts.Records.CreateRecordRequest;
using ApiCreateRecordResponse = Freem.Web.Api.Public.Contracts.Records.CreateRecordResponse;
using UseCaseCreateRecordRequest = Freem.Entities.UseCases.Contracts.Records.Create.CreateRecordRequest;
using UseCaseCreateRecordResponse = Freem.Entities.UseCases.Contracts.Records.Create.CreateRecordResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Authorize]
[Route("api/v1/records")]
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
    public async Task<ActionResult<ApiCreateRecordResponse>> CreateAsync(
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

    private static ApiCreateRecordResponse CreateSuccess(Record record)
    {
        return new ApiCreateRecordResponse(record.Id);
    }

    private static ActionResult<ApiCreateRecordResponse> CreateFailure(Error<CreateRecordErrorCode> error)
    {
        throw new NotImplementedException();
    }
}