using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Records.List;
using Freem.Linq;

namespace Freem.Entities.UseCases.Records;

internal class ListRecordUseCase 
    : IEntitiesUseCase<ListRecordRequest, ListRecordResponse, ListRecordErrorCode>
{
    private readonly IMultipleSearchByFilterRepository<Record, RecordIdentifier, RecordsByUserFilter> _repository;

    public ListRecordUseCase(
        IMultipleSearchByFilterRepository<Record, RecordIdentifier, RecordsByUserFilter> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<ListRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, ListRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var filter = new RecordsByUserFilter(context.UserId)
        {
            Limit = (int)request.Limit,
            Offset = (int)request.Offset
        };
        
        var result = await _repository.FindAsync(filter, cancellationToken);
        var records = await result.ToArrayAsync(cancellationToken);
        return ListRecordResponse.CreateSuccess(records, result.TotalCount);
    }
}