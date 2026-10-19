using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.RunningRecords.Get;

namespace Freem.Entities.UseCases.RunningRecords;

internal class GetRunningRecordUseCase 
    : IEntitiesUseCase<GetRunningRecordRequest, GetRunningRecordResponse, GetRunningRecordErrorCode>
{
    private readonly IRunningRecordRepository _repository;

    public GetRunningRecordUseCase(IRunningRecordRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<GetRunningRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, GetRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var id = RunningRecordId.FromUserId(context.UserId);
        var result = await _repository.FindByIdAsync(id, cancellationToken);
        return GetRunningRecordResponse.Create(result);
    }
}