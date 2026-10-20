using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.RunningRecords.Get;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.RunningRecords;

internal class GetRunningRecordUseCase 
    : IRequestHandler<HandlerContext, GetRunningRecordRequest, Result<GetRunningRecordResponse, Error>>
{
    private readonly IRunningRecordRepository _repository;

    public GetRunningRecordUseCase(IRunningRecordRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<GetRunningRecordResponse, Error>> HandleAsync(
        HandlerContext context, GetRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var id = RunningRecordId.FromUserId(context.UserId);
        var result = await _repository.FindByIdAsync(id, cancellationToken);
        if (!result.Success)
            return result.Error;

        return new GetRunningRecordResponse(result.Value);
    }
}