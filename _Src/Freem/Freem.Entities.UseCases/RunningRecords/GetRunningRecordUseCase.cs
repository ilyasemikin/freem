using Freem.Entities.RunningRecords;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.RunningRecords.Get;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.RunningRecords;

internal class GetRunningRecordUseCase 
    : IEntitiesUseCase<GetRunningRecordRequest, GetRunningRecordResponse, GetRunningRecordErrorCode>
{
    private readonly ISearchByIdRepository<RunningRecord, RunningRecordIdentifier> _repository;

    public GetRunningRecordUseCase(ISearchByIdRepository<RunningRecord, RunningRecordIdentifier> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<GetRunningRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, GetRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        return new GetRunningRecordResponse(result.Entity);
    }
}