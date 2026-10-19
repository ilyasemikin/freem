using Freem.Domain.Storage.Repositories.Records;
using Freem.Entities.UseCases.Contracts.Records.PeriodList;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Records;

internal sealed class PeriodListRecordUseCase : 
    IUseCase<UseCaseExecutionContext, PeriodListRequest, PeriodListResponse, PeriodListErrorCode>
{
    private readonly IRecordRepository _repository;

    public PeriodListRecordUseCase(
        IRecordRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<PeriodListResponse> ExecuteAsync(
        UseCaseExecutionContext context, PeriodListRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var filter = new RecordByPeriodFilter(context.UserId, request.Period);
        
        var result = await _repository.FindAsync(filter, cancellationToken);
        var records = await result.ToArrayAsync(cancellationToken);
        return PeriodListResponse.CreateSuccess(records, result.TotalCount);
    }
}