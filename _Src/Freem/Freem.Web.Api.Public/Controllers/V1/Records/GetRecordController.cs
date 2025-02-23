using System.ComponentModel.DataAnnotations;
using Freem.Entities.Identifiers;
using Freem.Entities.Records;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Records.Get;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Authorize]
[Route("api/v1/records/{recordId}")]
public sealed class GetRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;
    
    public GetRecordController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    public async Task<ActionResult<RecordResponse>> GetAsync(
        [Required] [FromRoute] string recordId,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(recordId);

        var response = await _executor.ExecuteAsync<GetRecordRequest, GetRecordResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Record)
            : CreateFailure(response.Error);
    }

    private static GetRecordRequest Map(string recordIdString)
    {
        var recordId = new RecordIdentifier(recordIdString);
        return new GetRecordRequest(recordId);
    }

    private static RecordResponse CreateSuccess(Record record)
    {
        return new RecordResponse(record.Id, record.Activities, record.Tags)
        {
            Name = record.Name,
            Description = record.Description
        };
    }

    private static ActionResult<RecordResponse> CreateFailure(Error<GetRecordErrorCode> error)
    {
        throw new NotImplementedException();
    }
}