using Freem.Domain.Storage.Repositories.Records;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Records.List;

namespace Freem.Entities.UseCases.Records;

internal sealed class ListRecordUseCase 
    : IEntitiesUseCase<ListRecordRequest, ListRecordResponse, ListRecordErrorCode>
{
    private readonly IRecordRepository _repository;

    public ListRecordUseCase(
        IRecordRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<ListRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, ListRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var filter = new RecordByUserFilter(context.UserId)
        {
            Limit = (int)request.Limit,
            Offset = (int)request.Offset
        };
        
        var result = await _repository.FindAsync(filter, cancellationToken);
        var records = await result.ToArrayAsync(cancellationToken);
        return ListRecordResponse.CreateSuccess(records, result.TotalCount);
    }
}