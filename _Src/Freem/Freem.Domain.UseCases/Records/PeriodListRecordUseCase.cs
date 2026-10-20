using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.PeriodList;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Records;

internal sealed class PeriodListRecordUseCase : 
    IRequestHandler<HandlerContext, PeriodListRequest, Result<PeriodListResponse, Error>>
{
    private readonly IRecordRepository _repository;

    public PeriodListRecordUseCase(
        IRecordRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<PeriodListResponse, Error>> HandleAsync(
        HandlerContext context, PeriodListRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = new RecordByPeriodFilter(context.UserId, request.Period);
        
        var result = await _repository.FindAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var records = await result.Value.ToArrayAsync(cancellationToken);
        return new PeriodListResponse(records, result.Value.Count);
    }
}