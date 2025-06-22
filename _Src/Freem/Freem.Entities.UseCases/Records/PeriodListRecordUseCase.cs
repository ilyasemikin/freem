using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.UseCases.Contracts.Records.PeriodList;
using Freem.Linq;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Records;

internal sealed class PeriodListRecordUseCase : 
    IUseCase<UseCaseExecutionContext, PeriodListRequest, PeriodListResponse, PeriodListErrorCode>
{
    private readonly IMultipleSearchByFilterRepository<Record, RecordIdentifier, RecordsByPeriodFilter> _repository;

    public PeriodListRecordUseCase(
        IMultipleSearchByFilterRepository<Record, RecordIdentifier, RecordsByPeriodFilter> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<PeriodListResponse> ExecuteAsync(
        UseCaseExecutionContext context, PeriodListRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var filter = new RecordsByPeriodFilter(context.UserId, request.Period);
        
        var result = await _repository.FindAsync(filter, cancellationToken);
        var records = await result.ToArrayAsync(cancellationToken);
        return PeriodListResponse.CreateSuccess(records, result.TotalCount);
    }
}